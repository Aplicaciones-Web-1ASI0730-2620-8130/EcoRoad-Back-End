namespace EcoRoad.Assets.Domain.Model.Commands;

/// <summary>
/// Registers the calibration of an installed sensor.
/// </summary>
/// <param name="SensorId">Identifier of the sensor.</param>
/// <param name="ValidUntil">Date until which the calibration is valid.</param>
public record CalibrateSensorCommand(string SensorId, DateTime ValidUntil);
