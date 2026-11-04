using EcoRoad.Assets.Domain.Model.ValueObjects;
using EcoRoad.Shared.Domain.Model.Entities;
using EcoRoad.Shared.Domain.Model.Exceptions;

namespace EcoRoad.Assets.Domain.Model.Aggregates;

/// <summary>
/// IoT sensor of the EcoRoad hardware as a service fleet.
/// </summary>
/// <remarks>
/// Deployment lifecycle: <c>Available</c>, assignment to a point, <c>Assigned</c>, calibration,
/// activation, <c>Active</c>. Deactivation returns the sensor to <c>Assigned</c> and release
/// returns it to <c>Available</c>. Only active sensors with a valid calibration can report.
/// </remarks>
public class Sensor : IAuditableEntity
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    private Sensor()
    {
    }

    /// <summary>
    /// Registers a new sensor in stock.
    /// </summary>
    /// <param name="name">Descriptive name of the device.</param>
    /// <param name="serialNumber">Manufacturer serial number; unique across the fleet.</param>
    /// <param name="type">Environmental variable the sensor captures.</param>
    /// <param name="model">Optional manufacturer model.</param>
    public Sensor(string name, string serialNumber, SensorType type, string? model)
    {
        Id = Guid.NewGuid().ToString();
        Code = AssetsRules.CodeFrom("SEN", Id);
        Name = AssetsRules.Require(name, "Name", 100, "INVALID_SENSOR");
        SerialNumber = AssetsRules.RequireSerialNumber(serialNumber);
        Type = AssetsRules.RequireSensorType(type);
        Model = AssetsRules.Optional(model, "Model", 60, "INVALID_SENSOR");
        Status = SensorStatus.Available;
    }

    /// <summary>Gets the unique identifier of the sensor.</summary>
    public string Id { get; private set; } = string.Empty;

    /// <summary>Gets the short human readable code of the sensor.</summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>Gets the descriptive name of the device.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets the normalized serial number.</summary>
    public string SerialNumber { get; private set; } = string.Empty;

    /// <summary>Gets the environmental variable captured by the sensor.</summary>
    public SensorType Type { get; private set; }

    /// <summary>Gets the manufacturer model, if known.</summary>
    public string? Model { get; private set; }

    /// <summary>Gets the deployment status.</summary>
    public SensorStatus Status { get; private set; }

    /// <summary>Gets the identifier of the monitoring point where the sensor is installed.</summary>
    public string? MonitoringPointId { get; private set; }

    /// <summary>Gets the identifier of the road project of the current point.</summary>
    public string? ProjectId { get; private set; }

    /// <summary>Gets the identifier of the road section of the current point.</summary>
    public string? SectionId { get; private set; }

    /// <summary>Gets the UTC date of the last calibration.</summary>
    public DateTime? CalibratedAt { get; private set; }

    /// <summary>Gets the UTC date until which the last calibration is valid.</summary>
    public DateTime? CalibrationValidUntil { get; private set; }

    /// <summary>Gets the UTC date when the sensor was activated.</summary>
    public DateTime? ActivatedAt { get; private set; }

    /// <inheritdoc />
    public DateTime? CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Checks whether the sensor holds a calibration that is still valid.
    /// </summary>
    /// <param name="now">Current UTC date and time.</param>
    /// <returns><c>true</c> when the calibration has not expired.</returns>
    public bool IsCalibrationValid(DateTime now)
    {
        return CalibrationValidUntil.HasValue && CalibrationValidUntil.Value > now;
    }

    /// <summary>
    /// Reporting policy: only active sensors linked to a point and with a valid calibration can report.
    /// </summary>
    /// <param name="now">Current UTC date and time.</param>
    /// <returns><c>true</c> when the measurements of the sensor can be accepted.</returns>
    public bool CanReport(DateTime now)
    {
        return Status == SensorStatus.Active
            && MonitoringPointId != null
            && ProjectId != null
            && IsCalibrationValid(now);
    }

    /// <summary>
    /// Installs the sensor at a monitoring point. Any previous calibration becomes invalid.
    /// </summary>
    /// <param name="point">The destination monitoring point.</param>
    public void AssignTo(MonitoringPoint point)
    {
        if (Status == SensorStatus.Active)
        {
            throw new AssetsRuleException("ACTIVE_SENSOR", "Deactivate the sensor before reassigning it.", DomainErrorKind.Conflict);
        }

        if (!point.IsEnabled)
        {
            throw new AssetsRuleException("POINT_DISABLED", "The monitoring point is disabled.", DomainErrorKind.Conflict);
        }

        if (MonitoringPointId == point.Id)
        {
            throw new AssetsRuleException("ALREADY_ASSIGNED", "The sensor is already assigned to this point.", DomainErrorKind.Conflict);
        }

        MonitoringPointId = point.Id;
        ProjectId = point.ProjectId;
        SectionId = point.SectionId;
        Status = SensorStatus.Assigned;
        ClearCalibration();
    }

    /// <summary>
    /// Registers a calibration of the installed sensor.
    /// </summary>
    /// <param name="validUntil">UTC date until which the calibration is valid.</param>
    /// <param name="now">Current UTC date and time.</param>
    public void Calibrate(DateTime validUntil, DateTime now)
    {
        if (Status != SensorStatus.Assigned || MonitoringPointId == null)
        {
            throw new AssetsRuleException("ASSIGNMENT_REQUIRED", "Assign the sensor to a point before calibrating it.", DomainErrorKind.Conflict);
        }

        if (validUntil <= now)
        {
            throw new AssetsRuleException("INVALID_CALIBRATION", "The calibration must be valid until a future date.");
        }

        CalibratedAt = now;
        CalibrationValidUntil = validUntil;
    }

    /// <summary>
    /// Authorizes the sensor to report measurements.
    /// </summary>
    /// <param name="now">Current UTC date and time.</param>
    public void Activate(DateTime now)
    {
        if (Status != SensorStatus.Assigned || MonitoringPointId == null)
        {
            throw new AssetsRuleException("ASSIGNMENT_REQUIRED", "Assign the sensor to a point before activating it.", DomainErrorKind.Conflict);
        }

        if (!IsCalibrationValid(now))
        {
            throw new AssetsRuleException("CALIBRATION_REQUIRED", "Calibrate the sensor before activating it.", DomainErrorKind.Conflict);
        }

        Status = SensorStatus.Active;
        ActivatedAt = now;
    }

    /// <summary>
    /// Stops the sensor from reporting while it stays installed.
    /// </summary>
    public void Deactivate()
    {
        if (Status != SensorStatus.Active)
        {
            throw new AssetsRuleException("SENSOR_NOT_ACTIVE", "Only active sensors can be deactivated.", DomainErrorKind.Conflict);
        }

        Status = SensorStatus.Assigned;
        ActivatedAt = null;
    }

    /// <summary>
    /// Uninstalls the sensor and returns it to stock.
    /// </summary>
    public void Release()
    {
        if (Status == SensorStatus.Active)
        {
            throw new AssetsRuleException("ACTIVE_SENSOR", "Deactivate the sensor before releasing it.", DomainErrorKind.Conflict);
        }

        if (MonitoringPointId == null)
        {
            throw new AssetsRuleException("SENSOR_NOT_ASSIGNED", "The sensor is not assigned.", DomainErrorKind.Conflict);
        }

        MonitoringPointId = null;
        ProjectId = null;
        SectionId = null;
        Status = SensorStatus.Available;
        ActivatedAt = null;
        ClearCalibration();
    }

    /// <summary>
    /// Removes the current calibration.
    /// </summary>
    private void ClearCalibration()
    {
        CalibratedAt = null;
        CalibrationValidUntil = null;
    }
}
