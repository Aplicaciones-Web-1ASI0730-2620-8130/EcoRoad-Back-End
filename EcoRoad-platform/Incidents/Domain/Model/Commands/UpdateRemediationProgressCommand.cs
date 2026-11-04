namespace EcoRoad.Incidents.Domain.Model.Commands;

/// <summary>
/// Updates the remediation progress of a corrective action.
/// </summary>
/// <param name="IncidentId">Identifier of the incident.</param>
/// <param name="ActionId">Identifier of the corrective action.</param>
/// <param name="ProgressPercentage">Progress from 0 to 100; 100 completes the action.</param>
public record UpdateRemediationProgressCommand(
    string IncidentId,
    string ActionId,
    int ProgressPercentage);
