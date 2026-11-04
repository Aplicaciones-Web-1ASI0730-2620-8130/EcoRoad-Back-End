using EcoRoad.Incidents.Domain.Model.ValueObjects;

namespace EcoRoad.Incidents.Interfaces.Rest.Resources;

/// <summary>
/// Request body to open an incident from an alert.
/// </summary>
/// <param name="CompanyId">Identifier of the company.</param>
/// <param name="AlertId">Identifier of the originating alert.</param>
/// <param name="ProjectId">Identifier of the road project.</param>
/// <param name="SectionId">Identifier of the road section.</param>
/// <param name="Title">Short title, usually the affected indicator.</param>
/// <param name="Description">Description of the observed environmental impact.</param>
/// <param name="Severity">Severity inherited from the alert.</param>
/// <param name="OpenedBy">Identifier of the user that opens the incident.</param>
public record OpenIncidentResource(
    string CompanyId,
    string AlertId,
    string ProjectId,
    string SectionId,
    string Title,
    string Description,
    IncidentSeverity Severity,
    string OpenedBy);

/// <summary>
/// Request body to assign the responsible person.
/// </summary>
/// <param name="UserId">Identifier of the responsible person.</param>
/// <param name="UserName">Name of the responsible person.</param>
public record AssignResponsibleResource(string UserId, string UserName);

/// <summary>
/// Request body to register a corrective action.
/// </summary>
/// <param name="Description">What must be done.</param>
/// <param name="AssignedBy">Identifier of the user that registers the action.</param>
public record RegisterCorrectiveActionResource(string Description, string AssignedBy);

/// <summary>
/// Request body to update the progress of a corrective action.
/// </summary>
/// <param name="ProgressPercentage">Progress from 0 to 100; 100 completes the action.</param>
public record UpdateRemediationProgressResource(int ProgressPercentage);

/// <summary>
/// Request body to record field evidence.
/// </summary>
/// <param name="Notes">Description of what the evidence shows.</param>
/// <param name="FileUrl">Optional address of the uploaded photograph or document.</param>
/// <param name="RegisteredBy">Identifier of the user that records the evidence.</param>
public record RecordFieldEvidenceResource(string Notes, string? FileUrl, string RegisteredBy);

/// <summary>
/// Request body to close an incident.
/// </summary>
/// <param name="ClosedBy">Identifier of the user that closes the incident.</param>
/// <param name="Notes">Optional closing notes.</param>
public record CloseIncidentResource(string ClosedBy, string? Notes);

/// <summary>
/// Corrective action returned by the API.
/// </summary>
/// <param name="Id">Identifier of the action.</param>
/// <param name="Description">What must be done.</param>
/// <param name="Status">Status of the action.</param>
/// <param name="RemediationProgress">Progress from 0 to 100.</param>
/// <param name="AssignedBy">User that registered the action.</param>
/// <param name="AssignedAt">UTC date when it was registered.</param>
/// <param name="StartedAt">UTC date when it started.</param>
/// <param name="ExecutedAt">UTC date when it was completed.</param>
public record CorrectiveActionResource(
    string Id,
    string Description,
    ActionStatus Status,
    int RemediationProgress,
    string AssignedBy,
    DateTime AssignedAt,
    DateTime? StartedAt,
    DateTime? ExecutedAt);

/// <summary>
/// Field evidence returned by the API.
/// </summary>
/// <param name="Id">Identifier of the evidence.</param>
/// <param name="Notes">Description of what the evidence shows.</param>
/// <param name="FileUrl">Address of the uploaded file.</param>
/// <param name="RegisteredByUserId">User that recorded it.</param>
/// <param name="RecordedAt">UTC date when it was recorded.</param>
public record FieldEvidenceResource(string Id, string Notes, string? FileUrl, string RegisteredByUserId, DateTime RecordedAt);

/// <summary>
/// Incident returned by the API, with its corrective actions and evidence.
/// </summary>
/// <param name="Id">Identifier of the incident.</param>
/// <param name="Code">Short human readable code.</param>
/// <param name="CompanyId">Identifier of the company.</param>
/// <param name="AlertId">Identifier of the originating alert.</param>
/// <param name="ProjectId">Identifier of the road project.</param>
/// <param name="SectionId">Identifier of the road section.</param>
/// <param name="Title">Short title.</param>
/// <param name="Description">Description of the impact.</param>
/// <param name="Severity">Severity inherited from the alert.</param>
/// <param name="Status">Current status.</param>
/// <param name="OpenedBy">User that opened it.</param>
/// <param name="OpenedAt">UTC opening date.</param>
/// <param name="ResponsibleUserId">Responsible person.</param>
/// <param name="ResponsibleName">Name of the responsible person.</param>
/// <param name="AssignedAt">UTC assignment date.</param>
/// <param name="AttendedAt">UTC attention date.</param>
/// <param name="ClosedBy">User that closed it.</param>
/// <param name="ClosureNotes">Closing notes.</param>
/// <param name="ClosedAt">UTC closing date.</param>
/// <param name="CorrectiveActions">Corrective actions.</param>
/// <param name="FieldEvidences">Field evidence.</param>
public record IncidentResource(
    string Id,
    string Code,
    string CompanyId,
    string AlertId,
    string ProjectId,
    string SectionId,
    string Title,
    string Description,
    IncidentSeverity Severity,
    IncidentStatus Status,
    string OpenedBy,
    DateTime OpenedAt,
    string? ResponsibleUserId,
    string? ResponsibleName,
    DateTime? AssignedAt,
    DateTime? AttendedAt,
    string? ClosedBy,
    string? ClosureNotes,
    DateTime? ClosedAt,
    IReadOnlyList<CorrectiveActionResource> CorrectiveActions,
    IReadOnlyList<FieldEvidenceResource> FieldEvidences);

/// <summary>
/// Number of incidents in every column of the board.
/// </summary>
/// <param name="Open">Incidents waiting for a responsible person.</param>
/// <param name="Assigned">Incidents under remediation.</param>
/// <param name="Attended">Incidents remediated and pending closure.</param>
/// <param name="Closed">Formally closed incidents.</param>
public record IncidentBoardSummaryResource(int Open, int Assigned, int Attended, int Closed);
