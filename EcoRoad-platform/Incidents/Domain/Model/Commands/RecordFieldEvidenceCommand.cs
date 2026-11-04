namespace EcoRoad.Incidents.Domain.Model.Commands;

/// <summary>
/// Records a piece of field evidence for an incident.
/// </summary>
/// <param name="IncidentId">Identifier of the incident.</param>
/// <param name="Notes">Description of what the evidence shows.</param>
/// <param name="FileUrl">Optional address of the uploaded photograph or document.</param>
/// <param name="RegisteredBy">Identifier of the user that records the evidence.</param>
public record RecordFieldEvidenceCommand(
    string IncidentId,
    string Notes,
    string? FileUrl,
    string RegisteredBy);
