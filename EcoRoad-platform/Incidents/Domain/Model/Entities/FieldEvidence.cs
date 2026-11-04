using EcoRoad.Shared.Domain.Model.Entities;

namespace EcoRoad.Incidents.Domain.Model.Entities;

/// <summary>
/// Verifiable evidence recorded on site, such as a photograph or a field report.
/// </summary>
/// <remarks>
/// Evidence is immutable once recorded and is only created through the <c>EnvironmentalIncident</c> aggregate.
/// </remarks>
public class FieldEvidence : IAuditableEntity
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    private FieldEvidence()
    {
    }

    /// <summary>
    /// Records a piece of field evidence.
    /// </summary>
    /// <param name="incidentId">Identifier of the incident.</param>
    /// <param name="notes">Description of what the evidence shows.</param>
    /// <param name="fileUrl">Optional address of the uploaded file.</param>
    /// <param name="registeredByUserId">Identifier of the user that records it.</param>
    /// <param name="now">Current UTC date and time.</param>
    internal FieldEvidence(string incidentId, string notes, string? fileUrl, string registeredByUserId, DateTime now)
    {
        Id = Guid.NewGuid().ToString();
        IncidentId = incidentId;
        Notes = IncidentsRules.Require(notes, "Notes", 1000, "EVIDENCE_REQUIRED");
        FileUrl = IncidentsRules.OptionalFileUrl(fileUrl);
        RegisteredByUserId = IncidentsRules.Require(registeredByUserId, "Registered by", 36, "INVALID_EVIDENCE");
        RecordedAt = now;
    }

    /// <summary>Gets the unique identifier of the evidence.</summary>
    public string Id { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the incident.</summary>
    public string IncidentId { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the user that recorded the evidence.</summary>
    public string RegisteredByUserId { get; private set; } = string.Empty;

    /// <summary>Gets the address of the uploaded file, if any.</summary>
    public string? FileUrl { get; private set; }

    /// <summary>Gets the description of what the evidence shows.</summary>
    public string Notes { get; private set; } = string.Empty;

    /// <summary>Gets the UTC date when the evidence was recorded.</summary>
    public DateTime RecordedAt { get; private set; }

    /// <inheritdoc />
    public DateTime? CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTime? UpdatedAt { get; set; }
}
