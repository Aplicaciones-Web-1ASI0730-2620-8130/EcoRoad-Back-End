namespace EcoRoad.Incidents.Domain.Model.Queries;

/// <summary>
/// Read model with the number of incidents in every column of the board.
/// </summary>
/// <param name="Open">Incidents waiting for a responsible person.</param>
/// <param name="Assigned">Incidents under remediation.</param>
/// <param name="Attended">Incidents remediated and pending closure.</param>
/// <param name="Closed">Formally closed incidents.</param>
public record IncidentBoardSummary(int Open, int Assigned, int Attended, int Closed);
