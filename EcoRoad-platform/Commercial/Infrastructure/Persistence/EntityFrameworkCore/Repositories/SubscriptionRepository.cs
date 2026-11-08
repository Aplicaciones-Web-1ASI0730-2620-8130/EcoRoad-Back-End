using EcoRoad.Commercial.Domain.Model.Aggregates;
using EcoRoad.Commercial.Domain.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Commercial.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly AppDbContext _context;

    public SubscriptionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Subscription?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Subscription>()
            .AsNoTracking()
            .FirstOrDefaultAsync(subscription => subscription.Id == id, cancellationToken);
    }

    public async Task<Subscription?> GetByCompanyAccountIdAsync(
        string companyAccountId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<Subscription>()
            .AsNoTracking()
            .FirstOrDefaultAsync(subscription => subscription.CompanyAccountId == companyAccountId, cancellationToken);
    }

    public async Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default)
    {
        await _context.Set<Subscription>().AddAsync(subscription, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Subscription subscription, CancellationToken cancellationToken = default)
    {
        _context.Set<Subscription>().Update(subscription);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
