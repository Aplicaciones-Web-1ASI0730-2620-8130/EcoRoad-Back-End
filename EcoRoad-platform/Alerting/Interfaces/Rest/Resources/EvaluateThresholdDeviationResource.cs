namespace EcoRoad.Alerting.Interfaces.Rest.Resources;

/// <summary>
///     Request body used to evaluate a measurement against its threshold.
/// </summary>
public record EvaluateThresholdDeviationResource(
    string MeasurementId,
    string MonitoringPointId,
    double MeasuredValue,
    double ThresholdValue,
    string Message);
