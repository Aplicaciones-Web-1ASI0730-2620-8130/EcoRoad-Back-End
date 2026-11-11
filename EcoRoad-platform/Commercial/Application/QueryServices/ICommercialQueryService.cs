using EcoRoad.Commercial.Domain.Model.Aggregates;
using EcoRoad.Commercial.Domain.Model.Queries;

namespace EcoRoad.Commercial.Application.QueryServices;

public interface ICommercialQueryService
{
    Task<CompanyAccount?> Handle(GetCompanyAccountQuery query, CancellationToken cancellationToken = default);

    Task<SubscriptionView?> Handle(GetSubscriptionViewQuery query, CancellationToken cancellationToken = default);
}
