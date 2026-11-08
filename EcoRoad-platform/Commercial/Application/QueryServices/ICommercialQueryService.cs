using EcoRoad.Commercial.Domain.Model.Queries;

namespace EcoRoad.Commercial.Application.QueryServices;

public interface ICommercialQueryService
{
    Task<SubscriptionView?> Handle(GetSubscriptionViewQuery query, CancellationToken cancellationToken = default);
}
