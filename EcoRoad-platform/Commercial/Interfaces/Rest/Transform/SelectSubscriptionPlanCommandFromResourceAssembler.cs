using EcoRoad.Commercial.Domain.Model.Aggregates;
using EcoRoad.Commercial.Domain.Model.Commands;
using EcoRoad.Commercial.Domain.Model.Queries;
using EcoRoad.Commercial.Interfaces.Rest.Resources;

namespace EcoRoad.Commercial.Interfaces.Rest.Transform;

public static class SelectSubscriptionPlanCommandFromResourceAssembler
{
    public static SelectSubscriptionPlanCommand ToCommandFromResource(
        string companyAccountId,
        SelectSubscriptionPlanResource resource)
    {
        return new SelectSubscriptionPlanCommand(companyAccountId, resource.Plan, resource.BillingCycle);
    }
}
