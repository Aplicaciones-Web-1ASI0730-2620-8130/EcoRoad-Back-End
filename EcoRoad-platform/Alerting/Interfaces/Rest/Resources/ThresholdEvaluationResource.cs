namespace EcoRoad.Alerting.Interfaces.Rest.Resources;

/// <summary>
///     Result of a threshold evaluation. The alert is present only when a deviation was raised.
/// </summary>
public record ThresholdEvaluationResource(
    bool DeviationDetected,
    bool IsCritical,
    DeviationAssessmentResource Assessment,
    AlertResource? Alert);
