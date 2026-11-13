using EcoRoad.Commercial.Domain.Model.ValueObjects;

namespace EcoRoad.Commercial.Interfaces.Rest.Resources;

public record SubscriptionViewResource(
    string CompanyAccountId,
    string CompanyName,
    string CompanyCode,
    CompanySegment Segment,
    bool HasPlatformAccess,
    string? SubscriptionId,
    SubscriptionPlan? Plan,
    BillingCycle? BillingCycle,
    SubscriptionStatus? Status,
    DateTime? SelectedAt,
    DateTime? ActivatedAt,
    DateTime? CurrentPeriodEnd);
