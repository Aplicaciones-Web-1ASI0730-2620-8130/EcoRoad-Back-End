namespace EcoRoad.Assets.Domain.Model.Commands;

/// <summary>
/// Authorizes a calibrated sensor to report measurements.
/// </summary>
/// <param name="SensorId">Identifier of the sensor.</param>
public record ActivateSensorCommand(string SensorId);
