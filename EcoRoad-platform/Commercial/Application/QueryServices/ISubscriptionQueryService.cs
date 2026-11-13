using EcoRoad.Commercial.Domain.Model.Queries;

namespace EcoRoad.Commercial.Application.QueryServices;

public interface ISubscriptionQueryService
{
    Task<SubscriptionView?> Handle(GetSubscriptionViewQuery query, CancellationToken cancellationToken = default);
}
