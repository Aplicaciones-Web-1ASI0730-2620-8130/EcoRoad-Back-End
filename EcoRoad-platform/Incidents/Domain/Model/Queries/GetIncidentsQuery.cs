using EcoRoad.Incidents.Domain.Model.ValueObjects;

namespace EcoRoad.Incidents.Domain.Model.Queries;

/// <summary>
/// Lists the incidents of the board with optional filters.
/// </summary>
/// <param name="CompanyId">Optional company filter.</param>
/// <param name="Status">Optional status filter.</param>
/// <param name="ProjectId">Optional road project filter.</param>
/// <param name="Search">Optional text matched against the code and the title.</param>
public record GetIncidentsQuery(
    string? CompanyId,
    IncidentStatus? Status,
    string? ProjectId,
    string? Search);
