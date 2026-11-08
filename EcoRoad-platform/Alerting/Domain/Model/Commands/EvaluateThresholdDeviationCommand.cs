namespace EcoRoad.Alerting.Domain.Model.Commands;

/// <summary>
///     Evaluates a measurement against its threshold.
///     Monitoring point and message are carried so a deviation can originate an alert.
/// </summary>
public record EvaluateThresholdDeviationCommand(
    string MeasurementId,
    string MonitoringPointId,
    double MeasuredValue,
    double ThresholdValue,
    string Message);
