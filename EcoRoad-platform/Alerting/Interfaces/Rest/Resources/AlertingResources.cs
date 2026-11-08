using EcoRoad.Alerting.Domain.Model.ValueObjects;

namespace EcoRoad.Alerting.Interfaces.Rest.Resources;

public record EvaluateThresholdDeviationResource(
    string MeasurementId,
    string MonitoringPointId,
    double MeasuredValue,
    double ThresholdValue,
    string Message);

public record AcknowledgeAlertResource(string AcknowledgedBy);

public record AlertResource(
    string Id,
    string AssessmentId,
    string MonitoringPointId,
    AlertSeverity Severity,
    AlertStatus Status,
    string Message,
    DateTime RaisedAt,
    DateTime? AcknowledgedAt,
    string? AcknowledgedBy);

public record ThresholdEvaluationResource(
    bool DeviationDetected,
    bool IsCritical,
    AlertResource? Alert);

public record ActiveAlertResource(
    string Id,
    string AssessmentId,
    string MonitoringPointId,
    AlertSeverity Severity,
    AlertStatus Status,
    string Message,
    DateTime RaisedAt,
    DateTime? AcknowledgedAt,
    string? AcknowledgedBy);
