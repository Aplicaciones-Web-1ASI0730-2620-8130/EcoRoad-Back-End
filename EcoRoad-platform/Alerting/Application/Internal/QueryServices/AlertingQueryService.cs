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
        var alerts = await _alerts.GetActiveAsync(cancellationToken);
        return alerts
            .Select(alert => new ActiveAlertView(
                alert.Id,
                alert.WorkFrontId,
                alert.Parameter,
                alert.Unit,
                alert.TimeBand,
                alert.MeasuredValue,
                alert.LimitValue,
                alert.Severity,
                alert.RaisedAt,
                alert.RequiresImmediateNotification))
            .ToList();
    }
}
