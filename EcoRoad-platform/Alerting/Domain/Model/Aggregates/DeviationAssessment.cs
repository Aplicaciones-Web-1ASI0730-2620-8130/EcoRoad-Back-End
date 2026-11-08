using EcoRoad.Alerting.Domain.Model;

namespace EcoRoad.Alerting.Domain.Model.Aggregates;

/// <summary>
///     Deviation assessment aggregate root for the Alerting bounded context.
///     Records the comparison of a measurement against its threshold.
/// </summary>
public class DeviationAssessment
{
    protected DeviationAssessment()
    {
        Id = string.Empty;
        MeasurementId = string.Empty;
    }

    /// <summary>
    ///     Evaluates a measurement against its threshold.
    ///     The deviation is critical when the measured value is greater than the threshold.
    /// </summary>
    public static DeviationAssessment EvaluateThresholdDeviation(
        string measurementId,
        double measuredValue,
        double thresholdValue)
    {
        var assessment = new DeviationAssessment
        {
            Id = Guid.NewGuid().ToString(),
            MeasurementId = AlertingRules.Require(measurementId, "Measurement id", 36),
            MeasuredValue = AlertingRules.RequireFinite(measuredValue, "Measured value"),
            ThresholdValue = AlertingRules.RequireFinite(thresholdValue, "Threshold value"),
            AssessedAt = DateTime.UtcNow
        };
        assessment.IsCritical = assessment.MeasuredValue > assessment.ThresholdValue;
        return assessment;
    }

    public string Id { get; private set; }

    public string MeasurementId { get; private set; }

    public double MeasuredValue { get; private set; }

    public double ThresholdValue { get; private set; }

    public bool IsCritical { get; private set; }

    public DateTime AssessedAt { get; private set; }

    /// <summary>
    ///     Confirms that the recorded deviation is critical.
    /// </summary>
    public void ConfirmCriticalDeviation()
    {
        if (MeasuredValue <= ThresholdValue)
        {
            throw new AlertingRuleException("A measurement within the threshold cannot be confirmed as critical.");
        }

        IsCritical = true;
    }
}
