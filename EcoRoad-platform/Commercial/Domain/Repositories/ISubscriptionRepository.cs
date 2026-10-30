using EcoRoad.Commercial.Domain.Model.Aggregates;

namespace EcoRoad.Commercial.Domain.Repositories;

public interface ISubscriptionRepository
{
    Task<Subscription?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<Subscription?> GetByCompanyAccountIdAsync(string companyAccountId, CancellationToken cancellationToken = default);

    Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default);

    Task UpdateAsync(Subscription subscription, CancellationToken cancellationToken = default);
}
