namespace EcoRoad.Alerting.Interfaces.Rest.Resources;

/// <summary>
///     Deviation assessment returned by the API.
/// </summary>
public record DeviationAssessmentResource(
    string Id,
    string MeasurementId,
    double MeasuredValue,
    double ThresholdValue,
    bool IsCritical,
    DateTime AssessedAt);
