namespace EcoRoad.Incidents.Domain.Model.Commands;

/// <summary>
/// Registers a corrective action for an incident.
/// </summary>
/// <param name="IncidentId">Identifier of the incident.</param>
/// <param name="Description">What must be done.</param>
/// <param name="AssignedBy">Identifier of the user that registers the action.</param>
public record RegisterCorrectiveActionCommand(
    string IncidentId,
    string Description,
    string AssignedBy);
