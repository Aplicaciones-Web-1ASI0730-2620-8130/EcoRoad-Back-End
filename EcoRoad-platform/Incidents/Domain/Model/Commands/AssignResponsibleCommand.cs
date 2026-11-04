namespace EcoRoad.Incidents.Domain.Model.Commands;

/// <summary>
/// Assigns the person responsible for the remediation of an incident.
/// </summary>
/// <param name="IncidentId">Identifier of the incident.</param>
/// <param name="UserId">Identifier of the responsible person.</param>
/// <param name="UserName">Name of the responsible person.</param>
public record AssignResponsibleCommand(
    string IncidentId,
    string UserId,
    string UserName);
