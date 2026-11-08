namespace EcoRoad.Alerting.Domain.Model.Commands;

public record EvaluateThresholdDeviationCommand(
    string MeasurementId,
    string MonitoringPointId,
    double MeasuredValue,
    double ThresholdValue,
    string Message);
