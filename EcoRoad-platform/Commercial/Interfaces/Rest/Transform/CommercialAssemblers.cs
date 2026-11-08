using EcoRoad.Commercial.Domain.Model.Aggregates;
using EcoRoad.Commercial.Domain.Model.Commands;
using EcoRoad.Commercial.Domain.Model.Queries;
using EcoRoad.Commercial.Interfaces.Rest.Resources;

namespace EcoRoad.Commercial.Interfaces.Rest.Transform;

public static class RegisterCompanyAccountCommandFromResourceAssembler
{
    public static RegisterCompanyAccountCommand ToCommandFromResource(RegisterCompanyAccountResource resource)
    {
        return new RegisterCompanyAccountCommand(resource.Name, resource.Code, resource.Segment);
    }
}

public static class SelectSubscriptionPlanCommandFromResourceAssembler
{
    public static SelectSubscriptionPlanCommand ToCommandFromResource(
        string companyAccountId,
        SelectSubscriptionPlanResource resource)
    {
        return new SelectSubscriptionPlanCommand(companyAccountId, resource.Plan, resource.BillingCycle);
    }
}

public static class CompanyAccountResourceFromEntityAssembler
{
    public static CompanyAccountResource ToResourceFromEntity(CompanyAccount account)
    {
        return new CompanyAccountResource(
            account.Id,
            account.Name,
            account.Code,
            account.Segment,
            account.Status,
            account.CreatedAt,
            account.HasPlatformAccess);
    }
}

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
