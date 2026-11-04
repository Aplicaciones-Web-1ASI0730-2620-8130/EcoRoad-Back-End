namespace EcoRoad.Assets.Domain.Model.Queries;

/// <summary>
/// Lists monitoring points, optionally limited to a road project.
/// </summary>
/// <param name="ProjectId">Optional road project filter.</param>
public record GetMonitoringPointsQuery(string? ProjectId);
