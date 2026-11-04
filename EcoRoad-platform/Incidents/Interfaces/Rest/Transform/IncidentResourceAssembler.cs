using EcoRoad.Incidents.Domain.Model.Aggregates;
using EcoRoad.Incidents.Domain.Model.Entities;
using EcoRoad.Incidents.Domain.Model.Queries;
using EcoRoad.Incidents.Interfaces.Rest.Resources;

namespace EcoRoad.Incidents.Interfaces.Rest.Transform;

/// <summary>
/// Converts incidents and their children into resources.
/// </summary>
public static class IncidentResourceAssembler
{
    /// <summary>
    /// Converts an incident into its resource.
    /// </summary>
    /// <param name="incident">The incident to convert.</param>
    /// <returns>The incident resource.</returns>
    public static IncidentResource ToResourceFromEntity(EnvironmentalIncident incident)
    {
        return new IncidentResource(
            incident.Id,
            incident.Code,
            incident.CompanyId,
            incident.AlertId,
            incident.ProjectId,
            incident.SectionId,
            incident.Title,
            incident.Description,
            incident.Severity,
            incident.Status,
            incident.OpenedBy,
            incident.OpenedAt,
            incident.ResponsibleUserId,
            incident.ResponsibleName,
            incident.AssignedAt,
            incident.AttendedAt,
            incident.ClosedBy,
            incident.ClosureNotes,
            incident.ClosedAt,
            incident.CorrectiveActions.OrderBy(action => action.AssignedAt).Select(ToActionResource).ToList(),
            incident.FieldEvidences.OrderBy(evidence => evidence.RecordedAt).Select(ToEvidenceResource).ToList());
    }

    /// <summary>
    /// Converts a corrective action into its resource.
    /// </summary>
    /// <param name="action">The action to convert.</param>
    /// <returns>The action resource.</returns>
    public static CorrectiveActionResource ToActionResource(CorrectiveAction action)
    {
        return new CorrectiveActionResource(
            action.Id,
            action.Description,
            action.Status,
            action.RemediationProgress,
            action.AssignedBy,
            action.AssignedAt,
            action.StartedAt,
            action.ExecutedAt);
    }

    /// <summary>
    /// Converts a piece of field evidence into its resource.
    /// </summary>
    /// <param name="evidence">The evidence to convert.</param>
    /// <returns>The evidence resource.</returns>
    public static FieldEvidenceResource ToEvidenceResource(FieldEvidence evidence)
    {
        return new FieldEvidenceResource(
            evidence.Id,
            evidence.Notes,
            evidence.FileUrl,
            evidence.RegisteredByUserId,
            evidence.RecordedAt);
    }

    /// <summary>
    /// Converts the board summary into its resource.
    /// </summary>
    /// <param name="summary">The board summary.</param>
    /// <returns>The summary resource.</returns>
    public static IncidentBoardSummaryResource ToSummaryResource(IncidentBoardSummary summary)
    {
        return new IncidentBoardSummaryResource(summary.Open, summary.Assigned, summary.Attended, summary.Closed);
    }
}
