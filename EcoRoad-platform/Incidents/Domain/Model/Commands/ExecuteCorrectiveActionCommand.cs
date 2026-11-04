namespace EcoRoad.Incidents.Domain.Model.Commands;

/// <summary>
/// Completes a corrective action.
/// </summary>
/// <param name="IncidentId">Identifier of the incident.</param>
/// <param name="ActionId">Identifier of the corrective action.</param>
public record ExecuteCorrectiveActionCommand(
    string IncidentId,
    string ActionId);
