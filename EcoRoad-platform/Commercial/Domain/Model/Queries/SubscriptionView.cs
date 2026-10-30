using EcoRoad.Commercial.Domain.Model.ValueObjects;

namespace EcoRoad.Commercial.Domain.Model.Queries;

public record SubscriptionView(
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
