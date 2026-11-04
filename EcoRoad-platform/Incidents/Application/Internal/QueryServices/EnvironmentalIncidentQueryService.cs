using EcoRoad.Incidents.Application.QueryServices;
using EcoRoad.Incidents.Domain.Model.Aggregates;
using EcoRoad.Incidents.Domain.Model.Queries;
using EcoRoad.Incidents.Domain.Model.ValueObjects;
using EcoRoad.Incidents.Domain.Repositories;

namespace EcoRoad.Incidents.Application.Internal.QueryServices;

/// <summary>
/// Answers the queries of the incident board.
/// </summary>
public class EnvironmentalIncidentQueryService : IEnvironmentalIncidentQueryService
{
    private readonly IEnvironmentalIncidentRepository _incidents;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvironmentalIncidentQueryService" /> class.
    /// </summary>
    /// <param name="incidents">Repository of incidents.</param>
    public EnvironmentalIncidentQueryService(IEnvironmentalIncidentRepository incidents)
    {
        _incidents = incidents;
    }

    /// <inheritdoc />
    public async Task<EnvironmentalIncident?> Handle(GetIncidentByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _incidents.FindByIdAsync(query.IncidentId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<EnvironmentalIncident>> Handle(
        GetIncidentsQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _incidents.ListByFiltersAsync(
            query.CompanyId,
            query.Status,
            query.ProjectId,
            query.Search,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IncidentBoardSummary> Handle(
        GetIncidentBoardSummaryQuery query,
        CancellationToken cancellationToken = default)
    {
        var counts = await _incidents.CountByStatusAsync(query.CompanyId, cancellationToken);
        int CountOf(IncidentStatus status) => counts.TryGetValue(status, out var count) ? count : 0;

        return new IncidentBoardSummary(
            CountOf(IncidentStatus.Open),
            CountOf(IncidentStatus.Assigned),
            CountOf(IncidentStatus.Attended),
            CountOf(IncidentStatus.Closed));
    }
}
