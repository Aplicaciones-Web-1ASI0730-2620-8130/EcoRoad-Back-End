using EcoRoad.Monitoring.Domain.Model.ValueObjects;

namespace EcoRoad.Monitoring.Domain.Model;

/// <summary>
/// Reusable guard clauses that protect the invariants of the monitoring aggregates.
/// </summary>
internal static class MonitoringRules
{
    /// <summary>Tolerance accepted for clocks of field devices that run slightly ahead.</summary>
    public static readonly TimeSpan ClockTolerance = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Ensures a text value is present and within the allowed length.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="field">Name of the field, used in the error message.</param>
    /// <param name="maxLength">Maximum number of characters allowed.</param>
    /// <param name="code">Error code raised when the value is invalid.</param>
    /// <returns>The trimmed value.</returns>
    public static string Require(string? value, string field, int maxLength, string code)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new MonitoringRuleException(code, $"{field} is required.");
        }

        var trimmed = value.Trim();
        if (trimmed.Length > maxLength)
        {
            throw new MonitoringRuleException(code, $"{field} exceeds {maxLength} characters.");
        }

        return trimmed;
    }

    /// <summary>
    /// Ensures an optional text value is within the allowed length.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="field">Name of the field, used in the error message.</param>
    /// <param name="maxLength">Maximum number of characters allowed.</param>
    /// <param name="code">Error code raised when the value is invalid.</param>
    /// <returns>The trimmed value, or <c>null</c> when it is empty.</returns>
    public static string? Optional(string? value, string field, int maxLength, string code)
    {
        return string.IsNullOrWhiteSpace(value) ? null : Require(value, field, maxLength, code);
    }

    /// <summary>
    /// Ensures an environmental parameter is supported.
    /// </summary>
    /// <param name="parameter">The parameter to validate.</param>
    /// <returns>The same parameter when it is supported.</returns>
    public static ParameterType RequireParameter(ParameterType parameter)
    {
        if (!Enum.IsDefined(parameter))
        {
            throw new MonitoringRuleException("INVALID_PARAMETER", "The environmental parameter is not supported.");
        }

        return parameter;
    }

    /// <summary>
    /// Ensures a measured value is physically possible for its parameter.
    /// </summary>
    /// <param name="parameter">The environmental parameter.</param>
    /// <param name="value">The measured value.</param>
    /// <returns>The same value when it is plausible.</returns>
    public static double RequirePlausibleValue(ParameterType parameter, double value)
    {
        if (!parameter.IsPlausible(value))
        {
            throw new MonitoringRuleException(
                "INVALID_MEASUREMENT",
                $"The value {value} is not a valid {parameter.Metric()} measurement.");
        }

        return value;
    }

    /// <summary>
    /// Ensures a measurement date is not in the future.
    /// </summary>
    /// <param name="recordedAt">UTC date of the measurement.</param>
    /// <param name="now">Current UTC date and time.</param>
    /// <returns>The same date when it is valid.</returns>
    public static DateTime RequirePastDate(DateTime recordedAt, DateTime now)
    {
        if (recordedAt > now.Add(ClockTolerance))
        {
            throw new MonitoringRuleException("INVALID_MEASUREMENT", "The measurement date cannot be in the future.");
        }

        return recordedAt;
    }

    /// <summary>
    /// Ensures a threshold value is a finite, non negative number.
    /// </summary>
    /// <param name="value">The threshold value.</param>
    /// <param name="field">Name of the field, used in the error message.</param>
    /// <returns>The same value when it is valid.</returns>
    public static double RequireThreshold(double? value, string field)
    {
        if (!value.HasValue || !double.IsFinite(value.Value) || value.Value < 0)
        {
            throw new MonitoringRuleException("INVALID_THRESHOLDS", $"{field} must be a non negative number.");
        }

        return value.Value;
    }
}
