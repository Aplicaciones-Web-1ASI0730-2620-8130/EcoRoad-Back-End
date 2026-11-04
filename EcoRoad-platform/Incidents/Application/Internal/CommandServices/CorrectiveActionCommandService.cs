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
/// Handles the corrective actions of an incident through its aggregate root.
/// </summary>
public class CorrectiveActionCommandService : ICorrectiveActionCommandService
{
    private readonly IEnvironmentalIncidentRepository _incidents;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CorrectiveActionCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CorrectiveActionCommandService" /> class.
    /// </summary>
    /// <param name="incidents">Repository of incidents.</param>
    /// <param name="unitOfWork">Unit of work that persists the changes.</param>
    /// <param name="logger">Logger used to trace the handled commands.</param>
    public CorrectiveActionCommandService(
        IEnvironmentalIncidentRepository incidents,
        IUnitOfWork unitOfWork,
        ILogger<CorrectiveActionCommandService> logger)
    {
        _incidents = incidents;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<CorrectiveAction> Handle(
        RegisterCorrectiveActionCommand command,
        CancellationToken cancellationToken = default)
    {
        var incident = await FindIncidentAsync(command.IncidentId, cancellationToken);
        var action = incident.AssignCorrectiveAction(command.Description, command.AssignedBy, DateTime.UtcNow);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Corrective action {ActionId} registered for incident {IncidentId}.", action.Id, incident.Id);
        return action;
    }

    /// <inheritdoc />
    public async Task<CorrectiveAction> Handle(
        StartCorrectiveActionCommand command,
        CancellationToken cancellationToken = default)
    {
        var incident = await FindIncidentAsync(command.IncidentId, cancellationToken);
        var action = incident.StartCorrectiveAction(command.ActionId, DateTime.UtcNow);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return action;
    }

    /// <inheritdoc />
    public async Task<CorrectiveAction> Handle(
        UpdateRemediationProgressCommand command,
        CancellationToken cancellationToken = default)
    {
        var incident = await FindIncidentAsync(command.IncidentId, cancellationToken);
        var action = incident.UpdateRemediationProgress(command.ActionId, command.ProgressPercentage, DateTime.UtcNow);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return action;
    }

    /// <inheritdoc />
    public async Task<CorrectiveAction> Handle(
        ExecuteCorrectiveActionCommand command,
        CancellationToken cancellationToken = default)
    {
        var incident = await FindIncidentAsync(command.IncidentId, cancellationToken);
        var action = incident.ExecuteCorrectiveAction(command.ActionId, DateTime.UtcNow);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Corrective action {ActionId} executed.", action.Id);
        return action;
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
