using EcoRoad.Incidents.Domain.Model.ValueObjects;
using EcoRoad.Shared.Domain.Model.Entities;
using EcoRoad.Shared.Domain.Model.Exceptions;

namespace EcoRoad.Incidents.Domain.Model.Entities;

/// <summary>
/// Corrective action executed to remediate an environmental incident, for example watering
/// the road or installing acoustic barriers.
/// </summary>
/// <remarks>
/// It is an entity of the <c>EnvironmentalIncident</c> aggregate and is only changed through it.
/// </remarks>
public class CorrectiveAction : IAuditableEntity
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    private CorrectiveAction()
    {
    }

    /// <summary>
    /// Registers a corrective action for an incident.
    /// </summary>
    /// <param name="incidentId">Identifier of the incident.</param>
    /// <param name="description">What must be done.</param>
    /// <param name="assignedBy">Identifier of the user that registers the action.</param>
    /// <param name="now">Current UTC date and time.</param>
    internal CorrectiveAction(string incidentId, string description, string assignedBy, DateTime now)
    {
        Id = Guid.NewGuid().ToString();
        IncidentId = incidentId;
        Description = IncidentsRules.Require(description, "Description", 500, "DESCRIPTION_REQUIRED");
        AssignedBy = IncidentsRules.Require(assignedBy, "Assigned by", 36, "INVALID_ACTION");
        AssignedAt = now;
        Status = ActionStatus.Assigned;
        RemediationProgress = 0;
    }

    /// <summary>Gets the unique identifier of the action.</summary>
    public string Id { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the incident.</summary>
    public string IncidentId { get; private set; } = string.Empty;

    /// <summary>Gets what must be done.</summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>Gets the status of the action.</summary>
    public ActionStatus Status { get; private set; }

    /// <summary>Gets the progress of the remediation, from 0 to 100.</summary>
    public int RemediationProgress { get; private set; }

    /// <summary>Gets the identifier of the user that registered the action.</summary>
    public string AssignedBy { get; private set; } = string.Empty;

    /// <summary>Gets the UTC date when the action was registered.</summary>
    public DateTime AssignedAt { get; private set; }

    /// <summary>Gets the UTC date when the action started.</summary>
    public DateTime? StartedAt { get; private set; }

    /// <summary>Gets the UTC date when the action was completed.</summary>
    public DateTime? ExecutedAt { get; private set; }

    /// <inheritdoc />
    public DateTime? CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets a value indicating whether the action was completed.
    /// </summary>
    public bool IsExecuted => Status == ActionStatus.Executed;

    /// <summary>
    /// Starts the execution of the action.
    /// </summary>
    /// <param name="now">Current UTC date and time.</param>
    internal void Start(DateTime now)
    {
        if (Status != ActionStatus.Assigned)
        {
            throw new IncidentsRuleException("INVALID_ACTION_STATUS", "Only a pending action can be started.", DomainErrorKind.Conflict);
        }

        Status = ActionStatus.InProgress;
        StartedAt = now;
    }

    /// <summary>
    /// Updates the remediation progress. Reaching 100 completes the action.
    /// </summary>
    /// <param name="progressPercentage">Progress from 0 to 100.</param>
    /// <param name="now">Current UTC date and time.</param>
    internal void UpdateProgress(int progressPercentage, DateTime now)
    {
        if (progressPercentage is < 0 or > 100)
        {
            throw new IncidentsRuleException("INVALID_PROGRESS", "Progress must be between 0 and 100.");
        }

        if (IsExecuted)
        {
            throw new IncidentsRuleException("INVALID_ACTION_STATUS", "The action was already executed.", DomainErrorKind.Conflict);
        }

        if (progressPercentage < RemediationProgress)
        {
            throw new IncidentsRuleException("INVALID_PROGRESS", "Progress cannot go backwards.");
        }

        if (Status == ActionStatus.Assigned)
        {
            Start(now);
        }

        RemediationProgress = progressPercentage;
        if (progressPercentage == 100)
        {
            Execute(now);
        }
    }

    /// <summary>
    /// Completes the action.
    /// </summary>
    /// <param name="now">Current UTC date and time.</param>
    internal void Execute(DateTime now)
    {
        if (IsExecuted)
        {
            throw new IncidentsRuleException("INVALID_ACTION_STATUS", "The action was already executed.", DomainErrorKind.Conflict);
        }

        StartedAt ??= now;
        Status = ActionStatus.Executed;
        RemediationProgress = 100;
        ExecutedAt = now;
    }
}
