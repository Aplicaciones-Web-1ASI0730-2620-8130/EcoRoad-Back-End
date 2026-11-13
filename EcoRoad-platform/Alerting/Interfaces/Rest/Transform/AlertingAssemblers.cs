using EcoRoad.Alerting.Domain.Model.Aggregates;
using EcoRoad.Alerting.Domain.Model.Commands;
using EcoRoad.Alerting.Domain.Model.Queries;
using EcoRoad.Alerting.Interfaces.Rest.Resources;

namespace EcoRoad.Alerting.Interfaces.Rest.Transform;

public static class RegisterAlertRuleCommandFromResourceAssembler
{
    public static RegisterAlertRuleCommand ToCommandFromResource(RegisterAlertRuleResource resource)
    {
        return new RegisterAlertRuleCommand(
            resource.Parameter,
            resource.Unit,
            resource.TimeBand,
            resource.Limit,
            resource.CriticalExcess);
    }
}

public static class EvaluateThresholdDeviationCommandFromResourceAssembler
{
    public static EvaluateThresholdDeviationCommand ToCommandFromResource(EvaluateThresholdDeviationResource resource)
    {
        return new EvaluateThresholdDeviationCommand(
            resource.WorkFrontId,
            resource.Parameter,
            resource.TimeBand,
            resource.MeasuredValue);
    }
}

public static class AlertRuleResourceFromEntityAssembler
{
    public static AlertRuleResource ToResourceFromEntity(AlertRule rule)
    {
        return new AlertRuleResource(
            rule.Id,
            rule.Parameter,
            rule.Unit,
            rule.TimeBand,
            rule.Limit,
            rule.CriticalExcess);
    }
}

public static class AlertResourceFromEntityAssembler
{
    public static AlertResource ToResourceFromEntity(Alert alert)
    {
        return new AlertResource(
            alert.Id,
            alert.WorkFrontId,
            alert.AlertRuleId,
            alert.Parameter,
            alert.Unit,
            alert.TimeBand,
            alert.MeasuredValue,
            alert.LimitValue,
            alert.Severity,
            alert.Status,
            alert.RaisedAt,
            alert.AcknowledgedAt,
            alert.AcknowledgedBy,
            alert.RequiresImmediateNotification);
    }
}

public static class ThresholdEvaluationResourceFromResultAssembler
{
    public static ThresholdEvaluationResource ToResourceFromResult(ThresholdEvaluationResult result)
    {
        return new ThresholdEvaluationResource(
            result.DeviationDetected,
            result.IsCritical,
            result.Alert == null ? null : AlertResourceFromEntityAssembler.ToResourceFromEntity(result.Alert));
    }
}

public static class ActiveAlertResourceFromViewAssembler
{
    public static ActiveAlertResource ToResourceFromView(ActiveAlertView view)
    {
        return new ActiveAlertResource(
            view.Id,
            view.WorkFrontId,
            view.Parameter,
            view.Unit,
            view.TimeBand,
            view.MeasuredValue,
            view.LimitValue,
            view.Severity,
            view.RaisedAt,
            view.RequiresImmediateNotification);
    }
}
