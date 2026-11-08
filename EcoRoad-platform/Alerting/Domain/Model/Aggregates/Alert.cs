using EcoRoad.Alerting.Domain.Model;
using EcoRoad.Alerting.Domain.Model.ValueObjects;

namespace EcoRoad.Alerting.Domain.Model.Aggregates;

/// <summary>
///     Alert aggregate root for the Alerting bounded context.
///     Each alert is originated by one deviation assessment.
/// </summary>
public class Alert
{
    protected Alert()
    {
        Id = string.Empty;
        AssessmentId = string.Empty;
        MonitoringPointId = string.Empty;
        Message = string.Empty;
    }

    /// <summary>
    ///     Raises an environmental alert originated by the given deviation assessment.
    /// </summary>
    public static Alert RaiseEnvironmentalAlert(
        string assessmentId,
        string monitoringPointId,
        AlertSeverity severity,
        string message)
    {
        if (!Enum.IsDefined(severity))
        {
            throw new AlertingRuleException("Severity is required.");
        }

        var alert = new Alert
        {
            Id = Guid.NewGuid().ToString(),
            AssessmentId = AlertingRules.Require(assessmentId, "Assessment id", 36),
            MonitoringPointId = AlertingRules.Require(monitoringPointId, "Monitoring point id", 36),
            Severity = severity,
            Status = AlertStatus.Raised,
            Message = AlertingRules.Require(message, "Message", 500),
            RaisedAt = DateTime.UtcNow
        };

        if (alert.Severity == AlertSeverity.Critical)
        {
            alert.NotifyResponsiblePersonnelOnCriticalAlert();
        }

        return alert;
    }

    public string Id { get; private set; }

    public string AssessmentId { get; private set; }

    public string MonitoringPointId { get; private set; }

    public AlertSeverity Severity { get; private set; }

    public AlertStatus Status { get; private set; }

    public string Message { get; private set; }

    public DateTime RaisedAt { get; private set; }

    public DateTime? AcknowledgedAt { get; private set; }

    public string? AcknowledgedBy { get; private set; }

    /// <summary>
    ///     Acknowledges the alert on behalf of the given user.
    /// </summary>
    public void AcknowledgeAlert(string userId)
    {
        if (Status == AlertStatus.Acknowledged)
        {
            throw new AlertingRuleException("The alert is already acknowledged.");
        }

        if (Status == AlertStatus.Closed)
        {
            throw new AlertingRuleException("A closed alert cannot be acknowledged.");
        }

        AcknowledgedBy = AlertingRules.Require(userId, "Acknowledged by", 100);
        AcknowledgedAt = DateTime.UtcNow;
        Status = AlertStatus.Acknowledged;
    }

    /// <summary>
    ///     Notifies responsible personnel when this alert is critical.
    /// </summary>
    public void NotifyResponsiblePersonnelOnCriticalAlert()
    {
        if (Severity != AlertSeverity.Critical)
        {
            throw new AlertingRuleException("Only a critical alert notifies responsible personnel.");
        }

        if (Status != AlertStatus.Raised)
        {
            throw new AlertingRuleException("Only a raised critical alert notifies responsible personnel.");
        }
    }
}
