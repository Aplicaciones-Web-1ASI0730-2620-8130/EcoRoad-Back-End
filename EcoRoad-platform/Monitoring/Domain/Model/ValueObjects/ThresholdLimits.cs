namespace EcoRoad.Monitoring.Domain.Model.ValueObjects;

/// <summary>
/// Limits used to classify the measurements of a parameter.
/// </summary>
/// <remarks>
/// <see cref="ThresholdKind.Maximum" /> uses <see cref="WarningThreshold" /> and <see cref="CriticalThreshold" />.
/// <see cref="ThresholdKind.Range" /> uses the four band limits, from <see cref="Minimum" /> to <see cref="Maximum" />.
/// </remarks>
/// <param name="Kind">Shape of the limits.</param>
/// <param name="WarningThreshold">Value from which a maximum profile becomes <c>Observation</c>.</param>
/// <param name="CriticalThreshold">Value from which a maximum profile becomes <c>Critical</c>.</param>
/// <param name="Minimum">Lowest acceptable value of a range profile.</param>
/// <param name="OptimalMinimum">Lowest optimal value of a range profile.</param>
/// <param name="OptimalMaximum">Highest optimal value of a range profile.</param>
/// <param name="Maximum">Highest acceptable value of a range profile.</param>
public record ThresholdLimits(
    ThresholdKind Kind,
    double? WarningThreshold,
    double? CriticalThreshold,
    double? Minimum,
    double? OptimalMinimum,
    double? OptimalMaximum,
    double? Maximum)
{
    /// <summary>
    /// Builds the limits of a maximum profile.
    /// </summary>
    /// <param name="warning">Value from which the status becomes <c>Observation</c>.</param>
    /// <param name="critical">Value from which the status becomes <c>Critical</c>.</param>
    /// <returns>The maximum limits.</returns>
    public static ThresholdLimits ForMaximum(double warning, double critical)
    {
        return new ThresholdLimits(ThresholdKind.Maximum, warning, critical, null, null, null, null);
    }

    /// <summary>
    /// Builds the limits of a range profile.
    /// </summary>
    /// <param name="minimum">Lowest acceptable value.</param>
    /// <param name="optimalMinimum">Lowest optimal value.</param>
    /// <param name="optimalMaximum">Highest optimal value.</param>
    /// <param name="maximum">Highest acceptable value.</param>
    /// <returns>The range limits.</returns>
    public static ThresholdLimits ForRange(double minimum, double optimalMinimum, double optimalMaximum, double maximum)
    {
        return new ThresholdLimits(ThresholdKind.Range, null, null, minimum, optimalMinimum, optimalMaximum, maximum);
    }
}
