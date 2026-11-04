using EcoRoad.Monitoring.Domain.Model.ValueObjects;
using EcoRoad.Shared.Domain.Model.Entities;
using EcoRoad.Shared.Domain.Model.Exceptions;

namespace EcoRoad.Monitoring.Domain.Model.Aggregates;

/// <summary>
/// Regulatory threshold profile used to classify the measurements of an environmental parameter.
/// </summary>
/// <remarks>
/// There is one profile per parameter. Limits are configured by the environmental team according
/// to the applicable environmental quality standards of the project.
/// </remarks>
public class ThresholdProfile : IAuditableEntity
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    private ThresholdProfile()
    {
    }

    /// <summary>
    /// Configures the threshold profile of an environmental parameter.
    /// </summary>
    /// <param name="name">Descriptive name, for example the standard it comes from.</param>
    /// <param name="parameterType">The environmental parameter.</param>
    /// <param name="limits">The limits used to classify the measurements.</param>
    public ThresholdProfile(string name, ParameterType parameterType, ThresholdLimits limits)
    {
        Id = Guid.NewGuid().ToString();
        ConfigureEnvironmentalParameter(name, parameterType);
        AssignEnvironmentalThresholds(limits);
    }

    /// <summary>Gets the unique identifier of the profile.</summary>
    public string Id { get; private set; } = string.Empty;

    /// <summary>Gets the descriptive name of the profile.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets the environmental parameter classified by the profile.</summary>
    public ParameterType ParameterType { get; private set; }

    /// <summary>Gets the unit of the limits.</summary>
    public string Unit { get; private set; } = string.Empty;

    /// <summary>Gets the shape of the limits.</summary>
    public ThresholdKind Kind { get; private set; }

    /// <summary>Gets the warning threshold of a maximum profile.</summary>
    public double? WarningThreshold { get; private set; }

    /// <summary>Gets the critical threshold of a maximum profile.</summary>
    public double? CriticalThreshold { get; private set; }

    /// <summary>Gets the lowest acceptable value of a range profile.</summary>
    public double? Minimum { get; private set; }

    /// <summary>Gets the lowest optimal value of a range profile.</summary>
    public double? OptimalMinimum { get; private set; }

    /// <summary>Gets the highest optimal value of a range profile.</summary>
    public double? OptimalMaximum { get; private set; }

    /// <summary>Gets the highest acceptable value of a range profile.</summary>
    public double? Maximum { get; private set; }

    /// <inheritdoc />
    public DateTime? CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Sets the parameter, its unit and the descriptive name of the profile.
    /// </summary>
    /// <param name="name">Descriptive name of the profile.</param>
    /// <param name="parameterType">The environmental parameter.</param>
    public void ConfigureEnvironmentalParameter(string name, ParameterType parameterType)
    {
        Name = MonitoringRules.Require(name, "Name", 100, "INVALID_THRESHOLDS");
        ParameterType = MonitoringRules.RequireParameter(parameterType);
        Unit = ParameterType.Unit();
    }

    /// <summary>
    /// Replaces the limits of the profile.
    /// </summary>
    /// <param name="limits">The new limits.</param>
    public void AssignEnvironmentalThresholds(ThresholdLimits limits)
    {
        switch (limits.Kind)
        {
            case ThresholdKind.Maximum:
                AssignMaximum(limits);
                break;
            case ThresholdKind.Range:
                AssignRange(limits);
                break;
            default:
                throw new MonitoringRuleException("INVALID_THRESHOLDS", "The threshold kind is not supported.");
        }
    }

    /// <summary>
    /// Gets the current limits of the profile.
    /// </summary>
    /// <returns>The limits.</returns>
    public ThresholdLimits Limits()
    {
        return new ThresholdLimits(Kind, WarningThreshold, CriticalThreshold, Minimum, OptimalMinimum, OptimalMaximum, Maximum);
    }

    /// <summary>
    /// Classifies a measured value against the limits of the profile.
    /// </summary>
    /// <param name="value">The measured value.</param>
    /// <returns>The monitoring status of the value.</returns>
    public MonitoringStatus Classify(double value)
    {
        if (Kind == ThresholdKind.Maximum)
        {
            if (value >= CriticalThreshold!.Value)
            {
                return MonitoringStatus.Critical;
            }

            return value >= WarningThreshold!.Value ? MonitoringStatus.Observation : MonitoringStatus.Optimal;
        }

        if (value < Minimum!.Value || value > Maximum!.Value)
        {
            return MonitoringStatus.Critical;
        }

        return value < OptimalMinimum!.Value || value > OptimalMaximum!.Value
            ? MonitoringStatus.Observation
            : MonitoringStatus.Optimal;
    }

    /// <summary>
    /// Validates and stores the limits of a maximum profile.
    /// </summary>
    /// <param name="limits">The limits.</param>
    private void AssignMaximum(ThresholdLimits limits)
    {
        var warning = MonitoringRules.RequireThreshold(limits.WarningThreshold, "Warning threshold");
        var critical = MonitoringRules.RequireThreshold(limits.CriticalThreshold, "Critical threshold");
        if (warning >= critical)
        {
            throw new MonitoringRuleException("INVALID_THRESHOLDS", "The warning threshold must be lower than the critical threshold.");
        }

        Kind = ThresholdKind.Maximum;
        WarningThreshold = warning;
        CriticalThreshold = critical;
        Minimum = null;
        OptimalMinimum = null;
        OptimalMaximum = null;
        Maximum = null;
    }

    /// <summary>
    /// Validates and stores the limits of a range profile.
    /// </summary>
    /// <param name="limits">The limits.</param>
    private void AssignRange(ThresholdLimits limits)
    {
        var minimum = MonitoringRules.RequireThreshold(limits.Minimum, "Minimum");
        var optimalMinimum = MonitoringRules.RequireThreshold(limits.OptimalMinimum, "Optimal minimum");
        var optimalMaximum = MonitoringRules.RequireThreshold(limits.OptimalMaximum, "Optimal maximum");
        var maximum = MonitoringRules.RequireThreshold(limits.Maximum, "Maximum");
        if (!(minimum <= optimalMinimum && optimalMinimum < optimalMaximum && optimalMaximum <= maximum))
        {
            throw new MonitoringRuleException(
                "INVALID_THRESHOLDS",
                "Range limits must satisfy minimum <= optimal minimum < optimal maximum <= maximum.");
        }

        Kind = ThresholdKind.Range;
        WarningThreshold = null;
        CriticalThreshold = null;
        Minimum = minimum;
        OptimalMinimum = optimalMinimum;
        OptimalMaximum = optimalMaximum;
        Maximum = maximum;
    }
}
