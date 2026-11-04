using EcoRoad.Monitoring.Domain.Model.ValueObjects;

namespace EcoRoad.Monitoring.Domain.Model.Queries;

/// <summary>
/// Read model with the current environmental situation of a road project.
/// </summary>
/// <param name="ProjectId">Identifier of the road project.</param>
/// <param name="Status">Worst current status of the project, or <c>null</c> without readings.</param>
/// <param name="LatestAt">UTC date of the most recent reading.</param>
/// <param name="Parameters">Indicator of every environmental parameter.</param>
/// <param name="Sections">Indicator of every section with readings.</param>
public record MonitoringDashboard(
    string ProjectId,
    MonitoringStatus? Status,
    DateTime? LatestAt,
    IReadOnlyList<ParameterIndicator> Parameters,
    IReadOnlyList<SectionIndicator> Sections);
