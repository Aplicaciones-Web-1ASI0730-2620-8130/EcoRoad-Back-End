using EcoRoad.Monitoring.Domain.Model.Aggregates;
using EcoRoad.Monitoring.Domain.Model.Commands;
using EcoRoad.Monitoring.Domain.Model.ValueObjects;
using EcoRoad.Monitoring.Interfaces.Rest.Resources;

namespace EcoRoad.Monitoring.Interfaces.Rest.Transform;

/// <summary>
/// Converts threshold profiles between their domain and REST representations.
/// </summary>
public static class ThresholdProfileAssembler
{
    /// <summary>
    /// Converts a threshold profile into its resource.
    /// </summary>
    /// <param name="profile">The profile to convert.</param>
    /// <returns>The profile resource.</returns>
    public static ThresholdProfileResource ToResourceFromEntity(ThresholdProfile profile)
    {
        return new ThresholdProfileResource(
            profile.Id,
            profile.Name,
            profile.ParameterType,
            profile.ParameterType.Metric(),
            profile.Unit,
            ToLimitsResource(profile.Limits()),
            profile.UpdatedAt);
    }

    /// <summary>
    /// Builds the configuration command.
    /// </summary>
    /// <param name="resource">The request body.</param>
    /// <returns>The command.</returns>
    public static ConfigureThresholdProfileCommand ToConfigureCommand(ConfigureThresholdProfileResource resource)
    {
        return new ConfigureThresholdProfileCommand(resource.Name, resource.ParameterType, ToLimits(resource.Limits));
    }

    /// <summary>
    /// Builds the thresholds update command.
    /// </summary>
    /// <param name="profileId">Identifier of the profile.</param>
    /// <param name="resource">The new limits.</param>
    /// <returns>The command.</returns>
    public static AssignThresholdsCommand ToAssignCommand(string profileId, ThresholdLimitsResource resource)
    {
        return new AssignThresholdsCommand(profileId, ToLimits(resource));
    }

    /// <summary>
    /// Converts limits into their resource.
    /// </summary>
    /// <param name="limits">The domain limits.</param>
    /// <returns>The limits resource.</returns>
    private static ThresholdLimitsResource ToLimitsResource(ThresholdLimits limits)
    {
        return new ThresholdLimitsResource(
            limits.Kind,
            limits.WarningThreshold,
            limits.CriticalThreshold,
            limits.Minimum,
            limits.OptimalMinimum,
            limits.OptimalMaximum,
            limits.Maximum);
    }

    /// <summary>
    /// Converts a limits resource into the domain value object.
    /// </summary>
    /// <param name="resource">The limits resource.</param>
    /// <returns>The domain limits.</returns>
    private static ThresholdLimits ToLimits(ThresholdLimitsResource resource)
    {
        return new ThresholdLimits(
            resource.Kind,
            resource.WarningThreshold,
            resource.CriticalThreshold,
            resource.Minimum,
            resource.OptimalMinimum,
            resource.OptimalMaximum,
            resource.Maximum);
    }
}
