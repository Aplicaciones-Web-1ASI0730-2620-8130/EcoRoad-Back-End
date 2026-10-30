using EcoRoad.Commercial.Domain.Model.ValueObjects;

namespace EcoRoad.Commercial.Interfaces.Rest.Resources;

public record RegisterCompanyAccountResource(string Name, string Code, CompanySegment Segment);

public record SelectSubscriptionPlanResource(SubscriptionPlan Plan, BillingCycle BillingCycle);

public record CompanyAccountResource(
    string Id,
    string Name,
    string Code,
    CompanySegment Segment,
    CompanyAccountStatus Status,
    DateTime CreatedAt,
    bool HasPlatformAccess);

public record SubscriptionResource(
    string Id,
    string CompanyAccountId,
    SubscriptionPlan Plan,
    BillingCycle BillingCycle,
    SubscriptionStatus Status,
    DateTime SelectedAt,
    DateTime? ActivatedAt,
    DateTime? CurrentPeriodEnd);

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
