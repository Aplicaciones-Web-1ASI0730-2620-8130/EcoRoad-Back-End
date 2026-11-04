namespace EcoRoad.Incidents.Domain.Model.Commands;

/// <summary>
/// Formally closes an attended incident.
/// </summary>
/// <param name="IncidentId">Identifier of the incident.</param>
/// <param name="ClosedBy">Identifier of the user that closes the incident.</param>
/// <param name="Notes">Optional closing notes.</param>
public record CloseIncidentCommand(
    string IncidentId,
    string ClosedBy,
    string? Notes);
