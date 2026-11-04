namespace EcoRoad.Assets.Domain.Model.Commands;

/// <summary>
/// Installs a sensor at a monitoring point.
/// </summary>
/// <param name="SensorId">Identifier of the sensor.</param>
/// <param name="MonitoringPointId">Identifier of the destination point.</param>
public record AssignSensorCommand(string SensorId, string MonitoringPointId);
