using EcoRoad.Commercial.Domain.Model.Aggregates;
using EcoRoad.Commercial.Domain.Model.Commands;
using EcoRoad.Commercial.Domain.Model.Queries;
using EcoRoad.Commercial.Interfaces.Rest.Resources;

namespace EcoRoad.Commercial.Interfaces.Rest.Transform;

public static class SubscriptionViewResourceFromQueryAssembler
{
    public static SubscriptionViewResource ToResourceFromView(SubscriptionView view)
    {
        return new SubscriptionViewResource(
            view.CompanyAccountId,
            view.CompanyName,
            view.CompanyCode,
            view.Segment,
            view.HasPlatformAccess,
            view.SubscriptionId,
            view.Plan,
            view.BillingCycle,
            view.Status,
            view.SelectedAt,
            view.ActivatedAt,
            view.CurrentPeriodEnd);
    }
}
