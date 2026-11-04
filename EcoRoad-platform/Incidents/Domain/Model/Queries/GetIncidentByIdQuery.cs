namespace EcoRoad.Incidents.Domain.Model.Queries;

/// <summary>
/// Retrieves an incident with its corrective actions and evidence.
/// </summary>
/// <param name="IncidentId">Identifier of the incident.</param>
public record GetIncidentByIdQuery(string IncidentId);
