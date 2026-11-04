namespace EcoRoad.Incidents.Domain.Model.Queries;

/// <summary>
/// Counts the incidents of the board by status.
/// </summary>
/// <param name="CompanyId">Optional company filter.</param>
public record GetIncidentBoardSummaryQuery(string? CompanyId);
