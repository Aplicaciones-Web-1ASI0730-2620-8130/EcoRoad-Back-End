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

    public async Task<Alert?> FindByIdAsync(string alertId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Alert>()
            .AsNoTracking()
            .FirstOrDefaultAsync(alert => alert.Id == alertId, cancellationToken);
    }

    public async Task<IReadOnlyList<Alert>> ListActiveAsync(CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, AlertStatus.Raised, cancellationToken);
    }

    public async Task<IReadOnlyList<Alert>> ListAsync(
        string? monitoringPointId,
        AlertStatus? status,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<Alert>().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(monitoringPointId))
        {
            query = query.Where(alert => alert.MonitoringPointId == monitoringPointId);
        }

        if (status.HasValue)
        {
            query = query.Where(alert => alert.Status == status.Value);
        }

        return await query
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
