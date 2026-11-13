using EcoRoad.Alerting.Domain.Model.ValueObjects;

namespace EcoRoad.Alerting.Domain.Model.Queries;

public record ActiveAlertView(
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
