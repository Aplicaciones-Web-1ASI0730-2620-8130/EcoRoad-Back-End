using EcoRoad.Commercial.Domain.Model.ValueObjects;

namespace EcoRoad.Commercial.Interfaces.Rest.Resources;

public record SubscriptionResource(
    string Id,
    string CompanyAccountId,
    SubscriptionPlan Plan,
    BillingCycle BillingCycle,
    SubscriptionStatus Status,
    DateTime SelectedAt,
    DateTime? ActivatedAt,
    DateTime? CurrentPeriodEnd);
