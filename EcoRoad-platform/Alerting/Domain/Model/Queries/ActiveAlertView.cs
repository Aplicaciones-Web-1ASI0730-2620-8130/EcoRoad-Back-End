using EcoRoad.Alerting.Domain.Model.ValueObjects;

namespace EcoRoad.Alerting.Domain.Model.Queries;

/// <summary>
///     Read model of an environmental alert.
/// </summary>
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
