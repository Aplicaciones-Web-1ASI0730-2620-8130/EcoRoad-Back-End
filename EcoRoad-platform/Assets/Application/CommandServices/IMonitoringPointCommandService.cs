using EcoRoad.Assets.Domain.Model.Aggregates;
using EcoRoad.Assets.Domain.Model.Commands;

namespace EcoRoad.Assets.Application.CommandServices;

/// <summary>
/// Application service that handles the commands of the <see cref="MonitoringPoint" /> aggregate.
/// </summary>
public interface IMonitoringPointCommandService
{
    /// <summary>Registers a monitoring point.</summary>
    /// <param name="command">The registration command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The new point.</returns>
    Task<MonitoringPoint> Handle(RegisterMonitoringPointCommand command, CancellationToken cancellationToken = default);

    /// <summary>Updates a monitoring point without installed sensors.</summary>
    /// <param name="command">The update command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The updated point.</returns>
    Task<MonitoringPoint> Handle(UpdateMonitoringPointCommand command, CancellationToken cancellationToken = default);

    /// <summary>Deletes a monitoring point without installed sensors.</summary>
    /// <param name="command">The delete command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    Task Handle(DeleteMonitoringPointCommand command, CancellationToken cancellationToken = default);

    /// <summary>Enables a monitoring point.</summary>
    /// <param name="command">The enable command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The updated point.</returns>
    Task<MonitoringPoint> Handle(EnableMonitoringPointCommand command, CancellationToken cancellationToken = default);

    /// <summary>Disables a monitoring point without installed sensors.</summary>
    /// <param name="command">The disable command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The updated point.</returns>
    Task<MonitoringPoint> Handle(DisableMonitoringPointCommand command, CancellationToken cancellationToken = default);
}
