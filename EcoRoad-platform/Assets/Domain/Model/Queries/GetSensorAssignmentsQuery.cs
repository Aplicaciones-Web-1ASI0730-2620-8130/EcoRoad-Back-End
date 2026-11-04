namespace EcoRoad.Assets.Domain.Model.Queries;

/// <summary>
/// Lists the installation history of a sensor.
/// </summary>
/// <param name="SensorId">Identifier of the sensor.</param>
public record GetSensorAssignmentsQuery(string SensorId);
