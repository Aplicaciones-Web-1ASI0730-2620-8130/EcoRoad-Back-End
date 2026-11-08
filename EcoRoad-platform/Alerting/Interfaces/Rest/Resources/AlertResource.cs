using EcoRoad.Alerting.Domain.Model.ValueObjects;

namespace EcoRoad.Alerting.Interfaces.Rest.Resources;

/// <summary>
///     Alert returned by the API.
/// </summary>
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
