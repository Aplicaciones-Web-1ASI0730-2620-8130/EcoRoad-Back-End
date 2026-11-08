using EcoRoad.Alerting.Domain.Model.ValueObjects;

namespace EcoRoad.Alerting.Domain.Model.Queries;

public record ActiveAlertView(
    string Id,
    string AssessmentId,
    string MonitoringPointId,
    AlertSeverity Severity,
    AlertStatus Status,
    string Message,
    DateTime RaisedAt,
    DateTime? AcknowledgedAt,
    string? AcknowledgedBy);
