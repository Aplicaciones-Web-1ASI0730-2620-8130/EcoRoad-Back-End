namespace EcoRoad.Assets.Domain.Model.Commands;

/// <summary>
/// Uninstalls a sensor and returns it to stock.
/// </summary>
/// <param name="SensorId">Identifier of the sensor.</param>
public record ReleaseSensorCommand(string SensorId);
