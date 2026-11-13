using EcoRoad.Commercial.Domain.Model.ValueObjects;

namespace EcoRoad.Commercial.Interfaces.Rest.Resources;

public record SelectSubscriptionPlanResource(SubscriptionPlan Plan, BillingCycle BillingCycle);
