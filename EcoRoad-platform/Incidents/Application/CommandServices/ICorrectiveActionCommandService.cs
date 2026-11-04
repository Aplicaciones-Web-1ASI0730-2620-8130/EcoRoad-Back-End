using EcoRoad.Incidents.Domain.Model.Commands;
using EcoRoad.Incidents.Domain.Model.Entities;

namespace EcoRoad.Incidents.Application.CommandServices;

/// <summary>
/// Application service that handles the corrective actions of an incident.
/// </summary>
public interface ICorrectiveActionCommandService
{
    /// <summary>Registers a corrective action.</summary>
    /// <param name="command">The registration command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The new corrective action.</returns>
    Task<CorrectiveAction> Handle(RegisterCorrectiveActionCommand command, CancellationToken cancellationToken = default);

    /// <summary>Starts a corrective action.</summary>
    /// <param name="command">The start command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The updated corrective action.</returns>
    Task<CorrectiveAction> Handle(StartCorrectiveActionCommand command, CancellationToken cancellationToken = default);

    /// <summary>Updates the progress of a corrective action.</summary>
    /// <param name="command">The progress command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The updated corrective action.</returns>
    Task<CorrectiveAction> Handle(UpdateRemediationProgressCommand command, CancellationToken cancellationToken = default);

    /// <summary>Completes a corrective action.</summary>
    /// <param name="command">The execution command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The updated corrective action.</returns>
    Task<CorrectiveAction> Handle(ExecuteCorrectiveActionCommand command, CancellationToken cancellationToken = default);
}
