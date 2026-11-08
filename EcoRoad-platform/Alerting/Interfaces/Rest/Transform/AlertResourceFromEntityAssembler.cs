using EcoRoad.Alerting.Domain.Model.Aggregates;
using EcoRoad.Alerting.Domain.Model.Queries;
using EcoRoad.Alerting.Interfaces.Rest.Resources;

namespace EcoRoad.Alerting.Interfaces.Rest.Transform;

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

    public static AlertResource ToResourceFromView(ActiveAlertView view)
    {
        return new AlertResource(
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

    public static IEnumerable<AlertResource> ToResourcesFromViews(IEnumerable<ActiveAlertView> views)
    {
        return views.Select(ToResourceFromView);
    }
}
