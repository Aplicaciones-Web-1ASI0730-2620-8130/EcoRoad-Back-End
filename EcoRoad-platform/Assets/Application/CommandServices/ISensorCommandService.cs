using EcoRoad.Assets.Domain.Model.Aggregates;
using EcoRoad.Assets.Domain.Model.Commands;

namespace EcoRoad.Assets.Application.CommandServices;

/// <summary>
/// Application service that handles the deployment lifecycle of the <see cref="Sensor" /> aggregate.
/// </summary>
public interface ISensorCommandService
{
    /// <summary>Registers a sensor in stock.</summary>
    /// <param name="command">The registration command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The new sensor.</returns>
    Task<Sensor> Handle(RegisterSensorCommand command, CancellationToken cancellationToken = default);

    /// <summary>Installs a sensor at a monitoring point.</summary>
    /// <param name="command">The assignment command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The updated sensor.</returns>
    Task<Sensor> Handle(AssignSensorCommand command, CancellationToken cancellationToken = default);

    /// <summary>Registers the calibration of a sensor.</summary>
    /// <param name="command">The calibration command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The updated sensor.</returns>
    Task<Sensor> Handle(CalibrateSensorCommand command, CancellationToken cancellationToken = default);

    /// <summary>Activates a sensor.</summary>
    /// <param name="command">The activation command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The updated sensor.</returns>
    Task<Sensor> Handle(ActivateSensorCommand command, CancellationToken cancellationToken = default);

    /// <summary>Deactivates a sensor.</summary>
    /// <param name="command">The deactivation command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The updated sensor.</returns>
    Task<Sensor> Handle(DeactivateSensorCommand command, CancellationToken cancellationToken = default);

    /// <summary>Releases a sensor back to stock.</summary>
    /// <param name="command">The release command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The updated sensor.</returns>
    Task<Sensor> Handle(ReleaseSensorCommand command, CancellationToken cancellationToken = default);
}
