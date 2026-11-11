using EcoRoad.Commercial.Domain.Model.Aggregates;
using EcoRoad.Commercial.Domain.Model.Commands;
using EcoRoad.Commercial.Domain.Model.Queries;
using EcoRoad.Commercial.Interfaces.Rest.Resources;

namespace EcoRoad.Commercial.Interfaces.Rest.Transform;

public static class SubscriptionResourceFromEntityAssembler
{
    public static SubscriptionResource ToResourceFromEntity(Subscription subscription)
    {
        return new SubscriptionResource(
            subscription.Id,
            subscription.CompanyAccountId,
            subscription.Plan,
            subscription.BillingCycle,
            subscription.Status,
            subscription.SelectedAt,
            subscription.ActivatedAt,
            subscription.CurrentPeriodEnd);
    }
}
