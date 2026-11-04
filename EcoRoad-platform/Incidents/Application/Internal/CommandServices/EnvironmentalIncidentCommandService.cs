using EcoRoad.Incidents.Application.CommandServices;
using EcoRoad.Incidents.Domain.Model;
using EcoRoad.Incidents.Domain.Model.Aggregates;
using EcoRoad.Incidents.Domain.Model.Commands;
using EcoRoad.Incidents.Domain.Model.Entities;
using EcoRoad.Incidents.Domain.Repositories;
using EcoRoad.Shared.Domain.Model.Exceptions;
using EcoRoad.Shared.Domain.Repositories;

namespace EcoRoad.Incidents.Application.Internal.CommandServices;

/// <summary>
/// Handles the lifecycle of environmental incidents: opening, assignment, evidence, attention and closure.
/// </summary>
public class EnvironmentalIncidentCommandService : IEnvironmentalIncidentCommandService
{
    private readonly IEnvironmentalIncidentRepository _incidents;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EnvironmentalIncidentCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvironmentalIncidentCommandService" /> class.
    /// </summary>
    /// <param name="incidents">Repository of incidents.</param>
    /// <param name="unitOfWork">Unit of work that persists the changes.</param>
    /// <param name="logger">Logger used to trace the handled commands.</param>
    public EnvironmentalIncidentCommandService(
        IEnvironmentalIncidentRepository incidents,
        IUnitOfWork unitOfWork,
        ILogger<EnvironmentalIncidentCommandService> logger)
    {
        _incidents = incidents;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<EnvironmentalIncident> Handle(OpenIncidentCommand command, CancellationToken cancellationToken = default)
    {
        var incident = EnvironmentalIncident.OpenEnvironmentalIncident(
            command.CompanyId,
            command.AlertId,
            command.ProjectId,
            command.SectionId,
            command.Title,
            command.Description,
            command.Severity,
            command.OpenedBy,
            DateTime.UtcNow);

        if (await _incidents.ExistsByAlertIdAsync(incident.AlertId, cancellationToken))
        {
            throw new IncidentsRuleException(
                "ALERT_ALREADY_HAS_INCIDENT",
                "The alert already originated an incident.",
                DomainErrorKind.Conflict);
        }

        await _incidents.AddAsync(incident, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Incident {IncidentId} opened from alert {AlertId}.", incident.Id, incident.AlertId);
        return incident;
    }

    /// <inheritdoc />
    public async Task<EnvironmentalIncident> Handle(AssignResponsibleCommand command, CancellationToken cancellationToken = default)
    {
        var incident = await FindIncidentAsync(command.IncidentId, cancellationToken);
        incident.AssignResponsiblePerson(command.UserId, command.UserName, DateTime.UtcNow);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Incident {IncidentId} assigned to {UserId}.", incident.Id, incident.ResponsibleUserId);
        return incident;
    }

    /// <inheritdoc />
    public async Task<FieldEvidence> Handle(RecordFieldEvidenceCommand command, CancellationToken cancellationToken = default)
    {
        var incident = await FindIncidentAsync(command.IncidentId, cancellationToken);
        var evidence = incident.RecordFieldEvidence(command.Notes, command.FileUrl, command.RegisteredBy, DateTime.UtcNow);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Evidence {EvidenceId} recorded for incident {IncidentId}.", evidence.Id, incident.Id);
        return evidence;
    }

    /// <inheritdoc />
    public async Task<EnvironmentalIncident> Handle(
        MarkIncidentAsAttendedCommand command,
        CancellationToken cancellationToken = default)
    {
        var incident = await FindIncidentAsync(command.IncidentId, cancellationToken);
        incident.MarkIncidentAsAttended(DateTime.UtcNow);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Incident {IncidentId} attended.", incident.Id);
        return incident;
    }

    /// <inheritdoc />
    public async Task<EnvironmentalIncident> Handle(CloseIncidentCommand command, CancellationToken cancellationToken = default)
    {
        var incident = await FindIncidentAsync(command.IncidentId, cancellationToken);
        incident.CloseIncident(command.ClosedBy, command.Notes, DateTime.UtcNow);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Incident {IncidentId} closed by {UserId}.", incident.Id, incident.ClosedBy);
        return incident;
    }

    /// <summary>
    /// Loads a whole incident or fails with <c>INCIDENT_NOT_FOUND</c>.
    /// </summary>
    /// <param name="incidentId">Identifier of the incident.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The incident.</returns>
    private async Task<EnvironmentalIncident> FindIncidentAsync(string incidentId, CancellationToken cancellationToken)
    {
        return await _incidents.FindByIdAsync(incidentId, cancellationToken)
            ?? throw new IncidentsRuleException("INCIDENT_NOT_FOUND", "The incident was not found.", DomainErrorKind.NotFound);
    }
}
