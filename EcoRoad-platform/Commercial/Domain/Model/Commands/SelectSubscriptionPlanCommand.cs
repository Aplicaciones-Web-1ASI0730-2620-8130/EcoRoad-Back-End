using EcoRoad.Commercial.Domain.Model.ValueObjects;

namespace EcoRoad.Commercial.Domain.Model.Commands;

public record SelectSubscriptionPlanCommand(
    string CompanyAccountId,
    SubscriptionPlan Plan,
    BillingCycle BillingCycle);
