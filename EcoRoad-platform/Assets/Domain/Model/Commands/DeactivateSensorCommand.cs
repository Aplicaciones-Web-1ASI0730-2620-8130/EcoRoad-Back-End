namespace EcoRoad.Assets.Domain.Model.Commands;

/// <summary>
/// Stops an active sensor from reporting while it stays installed.
/// </summary>
/// <param name="SensorId">Identifier of the sensor.</param>
public record DeactivateSensorCommand(string SensorId);
