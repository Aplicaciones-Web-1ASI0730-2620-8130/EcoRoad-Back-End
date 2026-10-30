using EcoRoad.Alerting.Domain.Model;
using EcoRoad.Alerting.Domain.Model.ValueObjects;

namespace EcoRoad.Alerting.Domain.Model.Aggregates;

public class Alert
{
    public Alert()
    {
    }

    public Alert(
        string workFrontId,
        string alertRuleId,
        string parameter,
        string unit,
        TimeBand timeBand,
        double measuredValue,
        double limitValue,
        AlertSeverity severity)
    {
        Id = Guid.NewGuid().ToString();
        WorkFrontId = AlertingRules.RequireId(workFrontId, "Work front id");
        AlertRuleId = AlertingRules.RequireId(alertRuleId, "Alert rule id");
        Parameter = AlertingRules.Require(parameter, "Parameter", 30);
        Unit = AlertingRules.Require(unit, "Unit", 20);
        TimeBand = timeBand;
        MeasuredValue = AlertingRules.RequireFinite(measuredValue, "Measured value");
        LimitValue = AlertingRules.RequireFinite(limitValue, "Limit");
        Severity = severity;
        Status = AlertStatus.Raised;
        RaisedAt = DateTime.UtcNow;
    }

    public string Id { get; set; } = string.Empty;

    public string WorkFrontId { get; set; } = string.Empty;

    public string AlertRuleId { get; set; } = string.Empty;

    public string Parameter { get; set; } = string.Empty;

    public string Unit { get; set; } = string.Empty;

    public TimeBand TimeBand { get; set; }

    public double MeasuredValue { get; set; }

    public double LimitValue { get; set; }

    public AlertSeverity Severity { get; set; }

    public AlertStatus Status { get; set; }

    public DateTime RaisedAt { get; set; }

    public DateTime? AcknowledgedAt { get; set; }

    public string? AcknowledgedBy { get; set; }

    public bool IsActive => Status == AlertStatus.Raised;

    public bool RequiresImmediateNotification => Severity == AlertSeverity.Critical && IsActive;

    public void Acknowledge(string acknowledgedBy)
    {
        if (Status == AlertStatus.Acknowledged)
        {
            throw new AlertingRuleException("The alert is already acknowledged.");
        }

        AcknowledgedBy = AlertingRules.Require(acknowledgedBy, "Acknowledged by", 100);
        AcknowledgedAt = DateTime.UtcNow;
        Status = AlertStatus.Acknowledged;
    }
}
