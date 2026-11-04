using EcoRoad.Incidents.Domain.Model.Commands;
using EcoRoad.Incidents.Interfaces.Rest.Resources;

namespace EcoRoad.Incidents.Interfaces.Rest.Transform;

/// <summary>
/// Converts incident request resources into commands.
/// </summary>
public static class IncidentCommandFromResourceAssembler
{
    /// <summary>Builds the open command.</summary>
    /// <param name="resource">The request body.</param>
    /// <returns>The command.</returns>
    public static OpenIncidentCommand ToOpenCommand(OpenIncidentResource resource)
    {
        return new OpenIncidentCommand(
            resource.CompanyId,
            resource.AlertId,
            resource.ProjectId,
            resource.SectionId,
            resource.Title,
            resource.Description,
            resource.Severity,
            resource.OpenedBy);
    }

    /// <summary>Builds the assignment command.</summary>
    /// <param name="incidentId">Identifier of the incident.</param>
    /// <param name="resource">The request body.</param>
    /// <returns>The command.</returns>
    public static AssignResponsibleCommand ToAssignCommand(string incidentId, AssignResponsibleResource resource)
    {
        return new AssignResponsibleCommand(incidentId, resource.UserId, resource.UserName);
    }

    /// <summary>Builds the corrective action registration command.</summary>
    /// <param name="incidentId">Identifier of the incident.</param>
    /// <param name="resource">The request body.</param>
    /// <returns>The command.</returns>
    public static RegisterCorrectiveActionCommand ToRegisterActionCommand(
        string incidentId,
        RegisterCorrectiveActionResource resource)
    {
        return new RegisterCorrectiveActionCommand(incidentId, resource.Description, resource.AssignedBy);
    }

    /// <summary>Builds the progress command.</summary>
    /// <param name="incidentId">Identifier of the incident.</param>
    /// <param name="actionId">Identifier of the action.</param>
    /// <param name="resource">The request body.</param>
    /// <returns>The command.</returns>
    public static UpdateRemediationProgressCommand ToProgressCommand(
        string incidentId,
        string actionId,
        UpdateRemediationProgressResource resource)
    {
        return new UpdateRemediationProgressCommand(incidentId, actionId, resource.ProgressPercentage);
    }

    /// <summary>Builds the evidence command.</summary>
    /// <param name="incidentId">Identifier of the incident.</param>
    /// <param name="resource">The request body.</param>
    /// <returns>The command.</returns>
    public static RecordFieldEvidenceCommand ToEvidenceCommand(string incidentId, RecordFieldEvidenceResource resource)
    {
        return new RecordFieldEvidenceCommand(incidentId, resource.Notes, resource.FileUrl, resource.RegisteredBy);
    }

    /// <summary>Builds the closure command.</summary>
    /// <param name="incidentId">Identifier of the incident.</param>
    /// <param name="resource">The request body.</param>
    /// <returns>The command.</returns>
    public static CloseIncidentCommand ToCloseCommand(string incidentId, CloseIncidentResource resource)
    {
        return new CloseIncidentCommand(incidentId, resource.ClosedBy, resource.Notes);
    }
}
