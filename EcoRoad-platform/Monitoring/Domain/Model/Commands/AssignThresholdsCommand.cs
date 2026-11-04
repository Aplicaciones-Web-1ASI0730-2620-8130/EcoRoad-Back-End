using EcoRoad.Monitoring.Domain.Model.ValueObjects;

namespace EcoRoad.Monitoring.Domain.Model.Commands;

/// <summary>
/// Replaces the limits of a threshold profile.
/// </summary>
/// <param name="ThresholdProfileId">Identifier of the profile.</param>
/// <param name="Limits">The new limits.</param>
public record AssignThresholdsCommand(
    string ThresholdProfileId,
    ThresholdLimits Limits);
