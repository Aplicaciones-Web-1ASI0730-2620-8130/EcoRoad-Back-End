using EcoRoad.Assets.Domain.Model.ValueObjects;

namespace EcoRoad.Assets.Interfaces.Rest.Resources;

/// <summary>
/// Request body to register or update a monitoring point.
/// </summary>
/// <param name="ProjectId">Identifier of the road project.</param>
/// <param name="SectionId">Identifier of the road section.</param>
/// <param name="Name">Descriptive name of the point.</param>
/// <param name="KilometerPost">Kilometer post, for example <c>PK 05+000</c>.</param>
/// <param name="Latitude">Latitude in decimal degrees.</param>
/// <param name="Longitude">Longitude in decimal degrees.</param>
public record SaveMonitoringPointResource(
    string ProjectId,
    string SectionId,
    string Name,
    string KilometerPost,
    double Latitude,
    double Longitude);

/// <summary>
/// Monitoring point returned by the API.
/// </summary>
/// <param name="Id">Identifier of the point.</param>
/// <param name="Code">Short human readable code.</param>
/// <param name="ProjectId">Identifier of the road project.</param>
/// <param name="SectionId">Identifier of the road section.</param>
/// <param name="Name">Descriptive name.</param>
/// <param name="KilometerPost">Canonical kilometer post.</param>
/// <param name="Latitude">Latitude in decimal degrees.</param>
/// <param name="Longitude">Longitude in decimal degrees.</param>
/// <param name="Status">Operational status.</param>
/// <param name="CreatedAt">UTC date when the point was registered.</param>
public record MonitoringPointResource(
    string Id,
    string Code,
    string ProjectId,
    string SectionId,
    string Name,
    string KilometerPost,
    double Latitude,
    double Longitude,
    MonitoringPointStatus Status,
    DateTime? CreatedAt);

/// <summary>
/// Request body to register a sensor.
/// </summary>
/// <param name="Name">Descriptive name of the device.</param>
/// <param name="SerialNumber">Manufacturer serial number, 3 to 40 letters, digits or hyphens.</param>
/// <param name="Type">Environmental variable the sensor captures.</param>
/// <param name="Model">Optional manufacturer model.</param>
public record RegisterSensorResource(string Name, string SerialNumber, SensorType Type, string? Model);

/// <summary>
/// Request body to install a sensor at a monitoring point.
/// </summary>
/// <param name="MonitoringPointId">Identifier of the destination point.</param>
public record AssignSensorResource(string MonitoringPointId);

/// <summary>
/// Request body to register a calibration.
/// </summary>
/// <param name="ValidUntil">Future date until which the calibration is valid.</param>
public record CalibrateSensorResource(DateTime ValidUntil);

/// <summary>
/// Sensor returned by the API.
/// </summary>
/// <param name="Id">Identifier of the sensor.</param>
/// <param name="Code">Short human readable code.</param>
/// <param name="Name">Descriptive name.</param>
/// <param name="SerialNumber">Normalized serial number.</param>
/// <param name="Type">Environmental variable captured.</param>
/// <param name="Model">Manufacturer model.</param>
/// <param name="Status">Deployment status.</param>
/// <param name="MonitoringPointId">Current monitoring point.</param>
/// <param name="ProjectId">Road project of the current point.</param>
/// <param name="SectionId">Road section of the current point.</param>
/// <param name="CalibratedAt">UTC date of the last calibration.</param>
/// <param name="CalibrationValidUntil">UTC date until which the calibration is valid.</param>
/// <param name="ActivatedAt">UTC date of the activation.</param>
/// <param name="CanReport">Whether the sensor is authorized to report measurements now.</param>
public record SensorResource(
    string Id,
    string Code,
    string Name,
    string SerialNumber,
    SensorType Type,
    string? Model,
    SensorStatus Status,
    string? MonitoringPointId,
    string? ProjectId,
    string? SectionId,
    DateTime? CalibratedAt,
    DateTime? CalibrationValidUntil,
    DateTime? ActivatedAt,
    bool CanReport);

/// <summary>
/// Entry of the installation history of a sensor.
/// </summary>
/// <param name="Id">Identifier of the assignment.</param>
/// <param name="SensorId">Identifier of the sensor.</param>
/// <param name="MonitoringPointId">Identifier of the point.</param>
/// <param name="AssignedAt">UTC date of the installation.</param>
/// <param name="ReleasedAt">UTC date when the sensor left the point.</param>
/// <param name="IsActive">Whether the sensor is still installed at the point.</param>
public record SensorAssignmentResource(
    string Id,
    string SensorId,
    string MonitoringPointId,
    DateTime AssignedAt,
    DateTime? ReleasedAt,
    bool IsActive);

/// <summary>
/// Deployment indicators of the sensor fleet.
/// </summary>
/// <param name="Total">Number of sensors.</param>
/// <param name="ReadyToReport">Sensors active, installed and calibrated.</param>
/// <param name="Assigned">Sensors installed but not active.</param>
/// <param name="Available">Sensors in stock.</param>
/// <param name="CalibrationDue">Installed sensors without a valid calibration.</param>
public record AssetSummaryResource(int Total, int ReadyToReport, int Assigned, int Available, int CalibrationDue);
