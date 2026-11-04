namespace EcoRoad.Assets.Domain.Model.Queries;

/// <summary>
/// Retrieves a sensor by its identifier.
/// </summary>
/// <param name="SensorId">Identifier of the sensor.</param>
public record GetSensorByIdQuery(string SensorId);
