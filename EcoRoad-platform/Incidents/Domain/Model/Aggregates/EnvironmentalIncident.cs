using EcoRoad.Incidents.Domain.Model.Entities;
using EcoRoad.Incidents.Domain.Model.ValueObjects;
using EcoRoad.Shared.Domain.Model.Entities;
using EcoRoad.Shared.Domain.Model.Exceptions;

namespace EcoRoad.Incidents.Domain.Model.Aggregates;

/// <summary>
/// Environmental incident opened from an alert and managed until its formal closure.
/// </summary>
/// <remarks>
/// Lifecycle: <c>Open</c>, responsible assigned, <c>Assigned</c>, corrective actions and field
/// evidence, <c>Attended</c>, formal closure, <c>Closed</c>. The aggregate owns its corrective
/// actions and evidence, so every change goes through its methods and keeps it consistent.
/// Alerting and Risk Evaluation keeps the original alert; the incident only references its identifier.
/// </remarks>
public class EnvironmentalIncident : IAuditableEntity
{
    private readonly List<CorrectiveAction> _correctiveActions = new();
    private readonly List<FieldEvidence> _fieldEvidences = new();

    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    private EnvironmentalIncident()
    {
    }

    /// <summary>Gets the unique identifier of the incident.</summary>
    public string Id { get; private set; } = string.Empty;

    /// <summary>Gets the short human readable code of the incident.</summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the company that owns the incident.</summary>
    public string CompanyId { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the alert that originated the incident.</summary>
    public string AlertId { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the road project.</summary>
    public string ProjectId { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the road section.</summary>
    public string SectionId { get; private set; } = string.Empty;

    /// <summary>Gets the short title of the incident, usually the affected indicator.</summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>Gets the description of the observed environmental impact.</summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>Gets the severity inherited from the alert.</summary>
    public IncidentSeverity Severity { get; private set; }

    /// <summary>Gets the current status of the incident.</summary>
    public IncidentStatus Status { get; private set; }

    /// <summary>Gets the identifier of the user that opened the incident.</summary>
    public string OpenedBy { get; private set; } = string.Empty;

    /// <summary>Gets the UTC date when the incident was opened.</summary>
    public DateTime OpenedAt { get; private set; }

    /// <summary>Gets the identifier of the responsible person.</summary>
    public string? ResponsibleUserId { get; private set; }

    /// <summary>Gets the name of the responsible person.</summary>
    public string? ResponsibleName { get; private set; }

    /// <summary>Gets the UTC date when the responsible person was assigned.</summary>
    public DateTime? AssignedAt { get; private set; }

    /// <summary>Gets the UTC date when the incident was marked as attended.</summary>
    public DateTime? AttendedAt { get; private set; }

    /// <summary>Gets the identifier of the user that closed the incident.</summary>
    public string? ClosedBy { get; private set; }

    /// <summary>Gets the closing notes.</summary>
    public string? ClosureNotes { get; private set; }

    /// <summary>Gets the UTC date when the incident was closed.</summary>
    public DateTime? ClosedAt { get; private set; }

    /// <inheritdoc />
    public DateTime? CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Gets the corrective actions of the incident.</summary>
    public IReadOnlyCollection<CorrectiveAction> CorrectiveActions => _correctiveActions;

    /// <summary>Gets the field evidence of the incident.</summary>
    public IReadOnlyCollection<FieldEvidence> FieldEvidences => _fieldEvidences;

    /// <summary>
    /// Opens an incident from an environmental alert.
    /// </summary>
    /// <param name="companyId">Identifier of the company.</param>
    /// <param name="alertId">Identifier of the originating alert.</param>
    /// <param name="projectId">Identifier of the road project.</param>
    /// <param name="sectionId">Identifier of the road section.</param>
    /// <param name="title">Short title, usually the affected indicator.</param>
    /// <param name="description">Description of the observed environmental impact.</param>
    /// <param name="severity">Severity inherited from the alert.</param>
    /// <param name="openedBy">Identifier of the user that opens the incident.</param>
    /// <param name="now">Current UTC date and time.</param>
    /// <returns>The open incident.</returns>
    public static EnvironmentalIncident OpenEnvironmentalIncident(
        string companyId,
        string alertId,
        string projectId,
        string sectionId,
        string title,
        string description,
        IncidentSeverity severity,
        string openedBy,
        DateTime now)
    {
        if (!Enum.IsDefined(severity))
        {
            throw new IncidentsRuleException("INVALID_INCIDENT", "Severity is not valid.");
        }

        var id = Guid.NewGuid().ToString();
        return new EnvironmentalIncident
        {
            Id = id,
            Code = IncidentsRules.CodeFrom("INC", id),
            CompanyId = IncidentsRules.Require(companyId, "Company id", 36, "INVALID_INCIDENT"),
            AlertId = IncidentsRules.Require(alertId, "Alert id", 36, "ALERT_REQUIRED"),
            ProjectId = IncidentsRules.Require(projectId, "Project id", 36, "INVALID_INCIDENT"),
            SectionId = IncidentsRules.Require(sectionId, "Section id", 36, "INVALID_INCIDENT"),
            Title = IncidentsRules.Require(title, "Title", 120, "INVALID_INCIDENT"),
            Description = IncidentsRules.Require(description, "Description", 1000, "DESCRIPTION_REQUIRED"),
            Severity = severity,
            Status = IncidentStatus.Open,
            OpenedBy = IncidentsRules.Require(openedBy, "Opened by", 36, "INVALID_INCIDENT"),
            OpenedAt = now
        };
    }

    /// <summary>
    /// Assigns, or reassigns, the person responsible for the remediation.
    /// </summary>
    /// <param name="userId">Identifier of the responsible person.</param>
    /// <param name="userName">Name of the responsible person.</param>
    /// <param name="now">Current UTC date and time.</param>
    public void AssignResponsiblePerson(string userId, string userName, DateTime now)
    {
        if (Status is not (IncidentStatus.Open or IncidentStatus.Assigned))
        {
            throw new IncidentsRuleException(
                "INVALID_STATUS",
                "Only an open or assigned incident can change its responsible person.",
                DomainErrorKind.Conflict);
        }

        ResponsibleUserId = IncidentsRules.Require(userId, "Responsible user id", 36, "INVALID_ASSIGNEE");
        ResponsibleName = IncidentsRules.Require(userName, "Responsible name", 100, "INVALID_ASSIGNEE");
        AssignedAt = now;
        Status = IncidentStatus.Assigned;
    }

    /// <summary>
    /// Registers a corrective action.
    /// </summary>
    /// <param name="description">What must be done.</param>
    /// <param name="assignedBy">Identifier of the user that registers the action.</param>
    /// <param name="now">Current UTC date and time.</param>
    /// <returns>The new corrective action.</returns>
    public CorrectiveAction AssignCorrectiveAction(string description, string assignedBy, DateTime now)
    {
        EnsureRemediationInProgress("Assign a responsible person before registering corrective actions.");
        var action = new CorrectiveAction(Id, description, assignedBy, now);
        _correctiveActions.Add(action);
        return action;
    }

    /// <summary>
    /// Starts the execution of a corrective action.
    /// </summary>
    /// <param name="actionId">Identifier of the action.</param>
    /// <param name="now">Current UTC date and time.</param>
    /// <returns>The updated corrective action.</returns>
    public CorrectiveAction StartCorrectiveAction(string actionId, DateTime now)
    {
        EnsureRemediationInProgress("Corrective actions can only change while the incident is assigned.");
        var action = FindAction(actionId);
        action.Start(now);
        return action;
    }

    /// <summary>
    /// Updates the remediation progress of a corrective action.
    /// </summary>
    /// <param name="actionId">Identifier of the action.</param>
    /// <param name="progressPercentage">Progress from 0 to 100; 100 completes the action.</param>
    /// <param name="now">Current UTC date and time.</param>
    /// <returns>The updated corrective action.</returns>
    public CorrectiveAction UpdateRemediationProgress(string actionId, int progressPercentage, DateTime now)
    {
        EnsureRemediationInProgress("Corrective actions can only change while the incident is assigned.");
        var action = FindAction(actionId);
        action.UpdateProgress(progressPercentage, now);
        return action;
    }

    /// <summary>
    /// Completes a corrective action.
    /// </summary>
    /// <param name="actionId">Identifier of the action.</param>
    /// <param name="now">Current UTC date and time.</param>
    /// <returns>The updated corrective action.</returns>
    public CorrectiveAction ExecuteCorrectiveAction(string actionId, DateTime now)
    {
        EnsureRemediationInProgress("Corrective actions can only change while the incident is assigned.");
        var action = FindAction(actionId);
        action.Execute(now);
        return action;
    }

    /// <summary>
    /// Records a piece of field evidence.
    /// </summary>
    /// <param name="notes">Description of what the evidence shows.</param>
    /// <param name="fileUrl">Optional address of the uploaded file.</param>
    /// <param name="registeredByUserId">Identifier of the user that records it.</param>
    /// <param name="now">Current UTC date and time.</param>
    /// <returns>The new field evidence.</returns>
    public FieldEvidence RecordFieldEvidence(string notes, string? fileUrl, string registeredByUserId, DateTime now)
    {
        EnsureRemediationInProgress("Field evidence requires an assigned incident.");
        var evidence = new FieldEvidence(Id, notes, fileUrl, registeredByUserId, now);
        _fieldEvidences.Add(evidence);
        return evidence;
    }

    /// <summary>
    /// Marks the incident as attended once every corrective action is executed and evidence exists.
    /// </summary>
    /// <param name="now">Current UTC date and time.</param>
    public void MarkIncidentAsAttended(DateTime now)
    {
        EnsureRemediationInProgress("Only an assigned incident can be marked as attended.");
        if (_correctiveActions.Count == 0 || _fieldEvidences.Count == 0)
        {
            throw new IncidentsRuleException(
                "REMEDIATION_INCOMPLETE",
                "Register at least one corrective action and one field evidence before marking the incident as attended.",
                DomainErrorKind.Conflict);
        }

        if (_correctiveActions.Any(action => !action.IsExecuted))
        {
            throw new IncidentsRuleException(
                "REMEDIATION_INCOMPLETE",
                "Every corrective action must be executed before marking the incident as attended.",
                DomainErrorKind.Conflict);
        }

        Status = IncidentStatus.Attended;
        AttendedAt = now;
    }

    /// <summary>
    /// Formally closes an attended incident.
    /// </summary>
    /// <param name="closedBy">Identifier of the user that closes the incident.</param>
    /// <param name="notes">Optional closing notes.</param>
    /// <param name="now">Current UTC date and time.</param>
    public void CloseIncident(string closedBy, string? notes, DateTime now)
    {
        if (Status != IncidentStatus.Attended)
        {
            throw new IncidentsRuleException(
                "INVALID_STATUS",
                "Only an attended incident can be closed.",
                DomainErrorKind.Conflict);
        }

        ClosedBy = IncidentsRules.Require(closedBy, "Closed by", 36, "INVALID_CLOSURE");
        ClosureNotes = IncidentsRules.Optional(notes, "Closure notes", 1000, "INVALID_CLOSURE");
        ClosedAt = now;
        Status = IncidentStatus.Closed;
    }

    /// <summary>
    /// Ensures the remediation phase is open, which happens while the incident is assigned.
    /// </summary>
    /// <param name="message">Message raised when it is not.</param>
    private void EnsureRemediationInProgress(string message)
    {
        if (Status != IncidentStatus.Assigned)
        {
            throw new IncidentsRuleException("INVALID_STATUS", message, DomainErrorKind.Conflict);
        }
    }

    /// <summary>
    /// Finds a corrective action of the incident.
    /// </summary>
    /// <param name="actionId">Identifier of the action.</param>
    /// <returns>The corrective action.</returns>
    private CorrectiveAction FindAction(string actionId)
    {
        return _correctiveActions.FirstOrDefault(action => action.Id == actionId)
            ?? throw new IncidentsRuleException(
                "ACTION_NOT_FOUND",
                "The corrective action was not found in this incident.",
                DomainErrorKind.NotFound);
    }
}
