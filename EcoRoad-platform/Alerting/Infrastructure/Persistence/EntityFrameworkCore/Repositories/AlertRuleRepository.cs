using EcoRoad.Alerting.Domain.Model.Aggregates;
using EcoRoad.Alerting.Domain.Model.ValueObjects;
using EcoRoad.Alerting.Domain.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Alerting.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class AlertRuleRepository : IAlertRuleRepository
{
    private readonly AppDbContext _context;

    public AlertRuleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AlertRule?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<AlertRule>()
            .AsNoTracking()
            .FirstOrDefaultAsync(rule => rule.Id == id, cancellationToken);
    }

    public async Task<AlertRule?> GetByParameterAndTimeBandAsync(
        string parameter,
        TimeBand timeBand,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<AlertRule>()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                rule => rule.Parameter == parameter && rule.TimeBand == timeBand,
                cancellationToken);
    }

    public async Task AddAsync(AlertRule rule, CancellationToken cancellationToken = default)
    {
        await _context.Set<AlertRule>().AddAsync(rule, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(AlertRule rule, CancellationToken cancellationToken = default)
    {
        _context.Set<AlertRule>().Update(rule);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
