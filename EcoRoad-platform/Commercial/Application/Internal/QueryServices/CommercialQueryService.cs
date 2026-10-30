using EcoRoad.Commercial.Application.QueryServices;
using EcoRoad.Commercial.Domain.Model.Queries;
using EcoRoad.Commercial.Domain.Repositories;

namespace EcoRoad.Commercial.Application.Internal.QueryServices;

public class CommercialQueryService : ICommercialQueryService
{
    private readonly ICompanyAccountRepository _accounts;
    private readonly ISubscriptionRepository _subscriptions;

    public CommercialQueryService(
        ICompanyAccountRepository accounts,
        ISubscriptionRepository subscriptions)
    {
        _accounts = accounts;
        _subscriptions = subscriptions;
    }

    public async Task<SubscriptionView?> Handle(
        GetSubscriptionViewQuery query,
        CancellationToken cancellationToken = default)
    {
        var account = await _accounts.GetByIdAsync(query.CompanyAccountId, cancellationToken);
        if (account == null)
        {
            return null;
        }

        var subscription = await _subscriptions.GetByCompanyAccountIdAsync(account.Id, cancellationToken);
        return new SubscriptionView(
            account.Id,
            account.Name,
            account.Code,
            account.Segment,
            account.HasPlatformAccess,
            subscription?.Id,
            subscription?.Plan,
            subscription?.BillingCycle,
            subscription?.Status,
            subscription?.SelectedAt,
            subscription?.ActivatedAt,
            subscription?.CurrentPeriodEnd);
    }
}
