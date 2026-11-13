using EcoRoad.Alerting.Domain.Model.ValueObjects;

namespace EcoRoad.Alerting.Interfaces.Rest.Resources;

public record RegisterAlertRuleResource(
    string Parameter,
    string Unit,
    TimeBand TimeBand,
    double Limit,
    double CriticalExcess);

public record EvaluateThresholdDeviationResource(
    string WorkFrontId,
    string Parameter,
    TimeBand TimeBand,
    double MeasuredValue);

public record AcknowledgeAlertResource(string AcknowledgedBy);

public record AlertRuleResource(
    string Id,
    string Parameter,
    string Unit,
    TimeBand TimeBand,
    double Limit,
    double CriticalExcess);

public record AlertResource(
    string Id,
    string WorkFrontId,
    string AlertRuleId,
    string Parameter,
    string Unit,
    TimeBand TimeBand,
    double MeasuredValue,
    double LimitValue,
    AlertSeverity Severity,
    AlertStatus Status,
    DateTime RaisedAt,
    DateTime? AcknowledgedAt,
    string? AcknowledgedBy,
    bool RequiresImmediateNotification);

public record ThresholdEvaluationResource(
    bool DeviationDetected,
    bool IsCritical,
    AlertResource? Alert);

public record ActiveAlertResource(
    string Id,
    string WorkFrontId,
    string Parameter,
    string Unit,
    TimeBand TimeBand,
    double MeasuredValue,
    double LimitValue,
    AlertSeverity Severity,
    DateTime RaisedAt,
    bool RequiresImmediateNotification);
