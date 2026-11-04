using EcoRoad.Assets.Application.CommandServices;
using EcoRoad.Assets.Domain.Model;
using EcoRoad.Assets.Domain.Model.Aggregates;
using EcoRoad.Assets.Domain.Model.Commands;
using EcoRoad.Assets.Domain.Repositories;
using EcoRoad.Shared.Domain.Model.Exceptions;
using EcoRoad.Shared.Domain.Repositories;

namespace EcoRoad.Assets.Application.Internal.CommandServices;

/// <summary>
/// Handles the deployment lifecycle of sensors: registration, assignment, calibration,
/// activation, deactivation and release.
/// </summary>
public class SensorCommandService : ISensorCommandService
{
    private readonly ISensorRepository _sensors;
    private readonly IMonitoringPointRepository _points;
    private readonly ISensorAssignmentRepository _assignments;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SensorCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SensorCommandService" /> class.
    /// </summary>
    /// <param name="sensors">Repository of sensors.</param>
    /// <param name="points">Repository of monitoring points.</param>
    /// <param name="assignments">Repository of the assignment history.</param>
    /// <param name="unitOfWork">Unit of work that persists the changes.</param>
    /// <param name="logger">Logger used to trace the handled commands.</param>
    public SensorCommandService(
        ISensorRepository sensors,
        IMonitoringPointRepository points,
        ISensorAssignmentRepository assignments,
        IUnitOfWork unitOfWork,
        ILogger<SensorCommandService> logger)
    {
        _sensors = sensors;
        _points = points;
        _assignments = assignments;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Sensor> Handle(RegisterSensorCommand command, CancellationToken cancellationToken = default)
    {
        var sensor = new Sensor(command.Name, command.SerialNumber, command.Type, command.Model);
        if (await _sensors.ExistsBySerialNumberAsync(sensor.SerialNumber, cancellationToken))
        {
            throw new AssetsRuleException(
                "DUPLICATE_SERIAL",
                "A sensor with this serial number is already registered.",
                DomainErrorKind.Conflict);
        }

        await _sensors.AddAsync(sensor, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Sensor {SensorId} registered with serial {Serial}.", sensor.Id, sensor.SerialNumber);
        return sensor;
    }

    /// <inheritdoc />
    public async Task<Sensor> Handle(AssignSensorCommand command, CancellationToken cancellationToken = default)
    {
        var sensor = await FindSensorAsync(command.SensorId, cancellationToken);
        var point = await _points.FindByIdAsync(command.MonitoringPointId, cancellationToken)
            ?? throw new AssetsRuleException("POINT_NOT_FOUND", "The monitoring point was not found.", DomainErrorKind.NotFound);

        if (await _sensors.ExistsTypeAtMonitoringPointAsync(point.Id, sensor.Type, sensor.Id, cancellationToken))
        {
            throw new AssetsRuleException(
                "POINT_TYPE_OCCUPIED",
                "The monitoring point already has a sensor of this type.",
                DomainErrorKind.Conflict);
        }

        var now = DateTime.UtcNow;
        sensor.AssignTo(point);
        await CloseOpenAssignmentAsync(sensor.Id, now, cancellationToken);
        await _assignments.AddAsync(new SensorAssignment(sensor.Id, point.Id, now), cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Sensor {SensorId} assigned to point {PointId}.", sensor.Id, point.Id);
        return sensor;
    }

    /// <inheritdoc />
    public async Task<Sensor> Handle(CalibrateSensorCommand command, CancellationToken cancellationToken = default)
    {
        var sensor = await FindSensorAsync(command.SensorId, cancellationToken);
        sensor.Calibrate(command.ValidUntil.ToUniversalTime(), DateTime.UtcNow);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return sensor;
    }

    /// <inheritdoc />
    public async Task<Sensor> Handle(ActivateSensorCommand command, CancellationToken cancellationToken = default)
    {
        var sensor = await FindSensorAsync(command.SensorId, cancellationToken);
        sensor.Activate(DateTime.UtcNow);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Sensor {SensorId} activated.", sensor.Id);
        return sensor;
    }

    /// <inheritdoc />
    public async Task<Sensor> Handle(DeactivateSensorCommand command, CancellationToken cancellationToken = default)
    {
        var sensor = await FindSensorAsync(command.SensorId, cancellationToken);
        sensor.Deactivate();
        await _unitOfWork.CompleteAsync(cancellationToken);
        return sensor;
    }

    /// <inheritdoc />
    public async Task<Sensor> Handle(ReleaseSensorCommand command, CancellationToken cancellationToken = default)
    {
        var sensor = await FindSensorAsync(command.SensorId, cancellationToken);
        sensor.Release();
        await CloseOpenAssignmentAsync(sensor.Id, DateTime.UtcNow, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Sensor {SensorId} released to stock.", sensor.Id);
        return sensor;
    }

    /// <summary>
    /// Loads a sensor or fails with <c>SENSOR_NOT_FOUND</c>.
    /// </summary>
    /// <param name="sensorId">Identifier of the sensor.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The sensor.</returns>
    private async Task<Sensor> FindSensorAsync(string sensorId, CancellationToken cancellationToken)
    {
        return await _sensors.FindByIdAsync(sensorId, cancellationToken)
            ?? throw new AssetsRuleException("SENSOR_NOT_FOUND", "The sensor was not found.", DomainErrorKind.NotFound);
    }

    /// <summary>
    /// Closes the open assignment of a sensor, if any.
    /// </summary>
    /// <param name="sensorId">Identifier of the sensor.</param>
    /// <param name="now">Current UTC date and time.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    private async Task CloseOpenAssignmentAsync(string sensorId, DateTime now, CancellationToken cancellationToken)
    {
        var open = await _assignments.FindActiveBySensorIdAsync(sensorId, cancellationToken);
        open?.Close(now);
    }
}
