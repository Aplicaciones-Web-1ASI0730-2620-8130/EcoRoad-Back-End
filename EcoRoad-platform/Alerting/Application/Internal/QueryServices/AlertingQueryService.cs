using EcoRoad.Alerting.Application.QueryServices;
using EcoRoad.Alerting.Domain.Model.Queries;
using EcoRoad.Alerting.Domain.Repositories;

namespace EcoRoad.Alerting.Application.Internal.QueryServices;

public class AlertingQueryService : IAlertingQueryService
{
    private readonly IAlertRepository _alerts;

    public AlertingQueryService(IAlertRepository alerts)
    {
        _alerts = alerts;
    }

    public async Task<IReadOnlyList<ActiveAlertView>> Handle(
        GetActiveAlertsQuery query,
        CancellationToken cancellationToken = default)
    {
        var alerts = await _alerts.ListActiveAsync(cancellationToken);
        return alerts
            .Select(alert => new ActiveAlertView(
                alert.Id,
                alert.AssessmentId,
                alert.MonitoringPointId,
                alert.Severity,
                alert.Status,
                alert.Message,
                alert.RaisedAt,
                alert.AcknowledgedAt,
                alert.AcknowledgedBy))
            .ToList();
    }
}
