using EcoRoad.Alerting.Domain.Model.Aggregates;
using EcoRoad.Alerting.Domain.Model.Commands;
using EcoRoad.Alerting.Domain.Model.Queries;
using EcoRoad.Alerting.Interfaces.Rest.Resources;

namespace EcoRoad.Alerting.Interfaces.Rest.Transform;

public static class EvaluateThresholdDeviationCommandFromResourceAssembler
{
    public static EvaluateThresholdDeviationCommand ToCommandFromResource(EvaluateThresholdDeviationResource resource)
    {
        return new EvaluateThresholdDeviationCommand(
            resource.MeasurementId,
            resource.MonitoringPointId,
            resource.MeasuredValue,
            resource.ThresholdValue,
            resource.Message);
    }
}

public static class AlertResourceFromEntityAssembler
{
    public static AlertResource ToResourceFromEntity(Alert alert)
    {
        return new AlertResource(
            alert.Id,
            alert.AssessmentId,
            alert.MonitoringPointId,
            alert.Severity,
            alert.Status,
            alert.Message,
            alert.RaisedAt,
            alert.AcknowledgedAt,
            alert.AcknowledgedBy);
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
            view.AssessmentId,
            view.MonitoringPointId,
            view.Severity,
            view.Status,
            view.Message,
            view.RaisedAt,
            view.AcknowledgedAt,
            view.AcknowledgedBy);
    }
}
