using EcoRoad.Assets.Domain.Model.ValueObjects;

namespace EcoRoad.Assets.Domain.Model.Queries;

/// <summary>
/// Lists the sensor inventory with optional filters.
/// </summary>
/// <param name="ProjectId">Optional road project filter.</param>
/// <param name="Status">Optional deployment status filter.</param>
/// <param name="Search">Optional text matched against the code, name and serial number.</param>
public record GetSensorsQuery(string? ProjectId, SensorStatus? Status, string? Search);
