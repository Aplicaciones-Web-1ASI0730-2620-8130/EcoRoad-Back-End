using EcoRoad.Alerting.Domain.Model.Queries;

namespace EcoRoad.Alerting.Application.QueryServices;

public interface IAlertingQueryService
{
    Task<IReadOnlyList<ActiveAlertView>> Handle(
        GetActiveAlertsQuery query,
        CancellationToken cancellationToken = default);
}
