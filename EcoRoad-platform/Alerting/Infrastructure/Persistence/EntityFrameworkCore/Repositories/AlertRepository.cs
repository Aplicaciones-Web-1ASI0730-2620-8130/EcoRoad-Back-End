using EcoRoad.Alerting.Domain.Model.Aggregates;
using EcoRoad.Alerting.Domain.Model.ValueObjects;
using EcoRoad.Alerting.Domain.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Alerting.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class AlertRepository : IAlertRepository
{
    private readonly AppDbContext _context;

    public AlertRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Alert?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Alert>()
            .AsNoTracking()
            .FirstOrDefaultAsync(alert => alert.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Alert>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<Alert>()
            .AsNoTracking()
            .Where(alert => alert.Status == AlertStatus.Raised)
            .OrderByDescending(alert => alert.RaisedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Alert alert, CancellationToken cancellationToken = default)
    {
        await _context.Set<Alert>().AddAsync(alert, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Alert alert, CancellationToken cancellationToken = default)
    {
        _context.Set<Alert>().Update(alert);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
