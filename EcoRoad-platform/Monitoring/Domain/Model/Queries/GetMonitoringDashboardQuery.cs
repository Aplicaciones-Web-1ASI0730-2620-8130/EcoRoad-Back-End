namespace EcoRoad.Monitoring.Domain.Model.Queries;

/// <summary>
/// Builds the current environmental dashboard of a road project.
/// </summary>
/// <param name="ProjectId">Identifier of the road project.</param>
public record GetMonitoringDashboardQuery(string ProjectId);
