using EcoRoad.Commercial.Domain.Model;
using EcoRoad.Commercial.Domain.Model.ValueObjects;

namespace EcoRoad.Commercial.Domain.Model.Aggregates;

public class Subscription
{
    public Subscription()
    {
    }

    public Subscription(string companyAccountId, SubscriptionPlan plan, BillingCycle billingCycle)
    {
        Id = Guid.NewGuid().ToString();
        CompanyAccountId = CommercialRules.Require(companyAccountId, "Company account id", 36);
        Plan = plan;
        BillingCycle = billingCycle;
        Status = SubscriptionStatus.Selected;
        SelectedAt = DateTime.UtcNow;
    }

    public string Id { get; set; } = string.Empty;

    public string CompanyAccountId { get; set; } = string.Empty;

    public SubscriptionPlan Plan { get; set; }

    public BillingCycle BillingCycle { get; set; }

    public SubscriptionStatus Status { get; set; }

    public DateTime SelectedAt { get; set; }

    public DateTime? ActivatedAt { get; set; }

    public DateTime? CurrentPeriodEnd { get; set; }

    public void SelectPlan(SubscriptionPlan plan, BillingCycle billingCycle)
    {
        if (Status != SubscriptionStatus.Selected)
        {
            throw new CommercialRuleException("The subscription plan can only be changed before activation.");
        }

        Plan = plan;
        BillingCycle = billingCycle;
        SelectedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (Status == SubscriptionStatus.Active)
        {
            throw new CommercialRuleException("The subscription is already active.");
        }

        var activatedAt = DateTime.UtcNow;
        Status = SubscriptionStatus.Active;
        ActivatedAt = activatedAt;
        CurrentPeriodEnd = EndOfCycle(activatedAt, BillingCycle);
    }

    public void Renew()
    {
        if (Status != SubscriptionStatus.Active || CurrentPeriodEnd == null)
        {
            throw new CommercialRuleException("Only an active subscription can be renewed.");
        }

        var periodStart = CurrentPeriodEnd.Value > DateTime.UtcNow
            ? CurrentPeriodEnd.Value
            : DateTime.UtcNow;

        CurrentPeriodEnd = EndOfCycle(periodStart, BillingCycle);
    }

    private static DateTime EndOfCycle(DateTime start, BillingCycle billingCycle)
    {
        return billingCycle == BillingCycle.Yearly
            ? start.AddYears(1)
            : start.AddMonths(1);
    }
}
