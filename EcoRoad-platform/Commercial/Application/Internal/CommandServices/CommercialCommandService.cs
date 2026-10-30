using EcoRoad.Commercial.Application.CommandServices;
using EcoRoad.Commercial.Domain.Model;
using EcoRoad.Commercial.Domain.Model.Aggregates;
using EcoRoad.Commercial.Domain.Model.Commands;
using EcoRoad.Commercial.Domain.Repositories;

namespace EcoRoad.Commercial.Application.Internal.CommandServices;

public class CommercialCommandService : ICommercialCommandService
{
    private readonly ICompanyAccountRepository _accounts;
    private readonly ISubscriptionRepository _subscriptions;

    public CommercialCommandService(
        ICompanyAccountRepository accounts,
        ISubscriptionRepository subscriptions)
    {
        _accounts = accounts;
        _subscriptions = subscriptions;
    }

    public async Task<CompanyAccount> Handle(
        RegisterCompanyAccountCommand command,
        CancellationToken cancellationToken = default)
    {
        var account = new CompanyAccount(command.Name, command.Code, command.Segment);
        var existing = await _accounts.GetByCodeAsync(account.Code, cancellationToken);
        if (existing != null)
        {
            throw new CommercialRuleException("A company account with this code is already registered.");
        }

        await _accounts.AddAsync(account, cancellationToken);
        return account;
    }

    public async Task<Subscription> Handle(
        SelectSubscriptionPlanCommand command,
        CancellationToken cancellationToken = default)
    {
        var account = await FindAccountAsync(command.CompanyAccountId, cancellationToken);
        var current = await _subscriptions.GetByCompanyAccountIdAsync(account.Id, cancellationToken);
        if (current == null)
        {
            var created = new Subscription(account.Id, command.Plan, command.BillingCycle);
            await _subscriptions.AddAsync(created, cancellationToken);
            return created;
        }

        current.SelectPlan(command.Plan, command.BillingCycle);
        await _subscriptions.UpdateAsync(current, cancellationToken);
        return current;
    }

    public async Task<Subscription> Handle(
        ActivateSubscriptionCommand command,
        CancellationToken cancellationToken = default)
    {
        var account = await FindAccountAsync(command.CompanyAccountId, cancellationToken);
        var subscription = await FindSubscriptionAsync(account.Id, cancellationToken);
        subscription.Activate();
        await _subscriptions.UpdateAsync(subscription, cancellationToken);

        account.EnableAccess();
        await _accounts.UpdateAsync(account, cancellationToken);
        return subscription;
    }

    public async Task<Subscription> Handle(
        RenewSubscriptionCommand command,
        CancellationToken cancellationToken = default)
    {
        var account = await FindAccountAsync(command.CompanyAccountId, cancellationToken);
        var subscription = await FindSubscriptionAsync(account.Id, cancellationToken);
        subscription.Renew();
        await _subscriptions.UpdateAsync(subscription, cancellationToken);
        return subscription;
    }

    private async Task<CompanyAccount> FindAccountAsync(string companyAccountId, CancellationToken cancellationToken)
    {
        var account = await _accounts.GetByIdAsync(companyAccountId, cancellationToken);
        if (account == null)
        {
            throw new CommercialRuleException("Company account was not found.", notFound: true);
        }

        return account;
    }

    private async Task<Subscription> FindSubscriptionAsync(string companyAccountId, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptions.GetByCompanyAccountIdAsync(companyAccountId, cancellationToken);
        if (subscription == null)
        {
            throw new CommercialRuleException("Subscription was not found.", notFound: true);
        }

        return subscription;
    }
}
