using EcoRoad.Incidents.Domain.Model.Aggregates;
using EcoRoad.Incidents.Domain.Model.Queries;

namespace EcoRoad.Incidents.Application.QueryServices;

/// <summary>
/// Application service that answers the queries of the incident board.
/// </summary>
public interface IEnvironmentalIncidentQueryService
{
    /// <summary>Retrieves an incident.</summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The incident when it exists; otherwise <c>null</c>.</returns>
    Task<EnvironmentalIncident?> Handle(GetIncidentByIdQuery query, CancellationToken cancellationToken = default);

    /// <summary>Lists the incidents of the board.</summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The matching incidents.</returns>
    Task<IReadOnlyList<EnvironmentalIncident>> Handle(GetIncidentsQuery query, CancellationToken cancellationToken = default);

    /// <summary>Counts the incidents per status.</summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The board summary.</returns>
    Task<IncidentBoardSummary> Handle(GetIncidentBoardSummaryQuery query, CancellationToken cancellationToken = default);
}
