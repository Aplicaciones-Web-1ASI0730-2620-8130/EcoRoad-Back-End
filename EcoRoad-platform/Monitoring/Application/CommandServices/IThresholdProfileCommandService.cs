using EcoRoad.Monitoring.Domain.Model.Aggregates;
using EcoRoad.Monitoring.Domain.Model.Commands;

namespace EcoRoad.Monitoring.Application.CommandServices;

/// <summary>
/// Application service that handles the commands of the <see cref="ThresholdProfile" /> aggregate.
/// </summary>
public interface IThresholdProfileCommandService
{
    /// <summary>Configures the threshold profile of a parameter.</summary>
    /// <param name="command">The configuration command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The new profile.</returns>
    Task<ThresholdProfile> Handle(ConfigureThresholdProfileCommand command, CancellationToken cancellationToken = default);

    /// <summary>Replaces the limits of a profile.</summary>
    /// <param name="command">The thresholds command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The updated profile.</returns>
    Task<ThresholdProfile> Handle(AssignThresholdsCommand command, CancellationToken cancellationToken = default);
}
