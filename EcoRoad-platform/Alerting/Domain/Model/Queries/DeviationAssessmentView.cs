namespace EcoRoad.Alerting.Domain.Model.Queries;

/// <summary>
///     Read model of a deviation assessment.
/// </summary>
public record DeviationAssessmentView(
    string Id,
    string MeasurementId,
    double MeasuredValue,
    double ThresholdValue,
    bool IsCritical,
    DateTime AssessedAt);
