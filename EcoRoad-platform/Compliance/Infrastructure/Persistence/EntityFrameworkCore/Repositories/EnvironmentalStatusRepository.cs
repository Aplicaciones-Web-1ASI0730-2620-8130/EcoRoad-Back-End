using EcoRoad.Compliance.Domain.Model.Aggregates;
using EcoRoad.Compliance.Domain.Model.ValueObjects;
using EcoRoad.Compliance.Domain.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Compliance.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class EnvironmentalStatusRepository : IEnvironmentalStatusRepository
{
    private readonly AppDbContext _context;

    public EnvironmentalStatusRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EnvironmentalStatus?> GetBySubjectAsync(
        ComplianceScope scope,
        string subjectId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<EnvironmentalStatus>()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                status => status.Scope == scope && status.SubjectId == subjectId,
                cancellationToken);
    }

    public async Task AddAsync(EnvironmentalStatus status, CancellationToken cancellationToken = default)
    {
        await _context.Set<EnvironmentalStatus>().AddAsync(status, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(EnvironmentalStatus status, CancellationToken cancellationToken = default)
    {
        _context.Set<EnvironmentalStatus>().Update(status);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
