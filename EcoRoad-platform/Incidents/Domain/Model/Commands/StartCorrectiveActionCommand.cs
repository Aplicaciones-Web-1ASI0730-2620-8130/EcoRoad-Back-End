namespace EcoRoad.Incidents.Domain.Model.Commands;

/// <summary>
/// Starts the execution of a corrective action.
/// </summary>
/// <param name="IncidentId">Identifier of the incident.</param>
/// <param name="ActionId">Identifier of the corrective action.</param>
public record StartCorrectiveActionCommand(
    string IncidentId,
    string ActionId);
