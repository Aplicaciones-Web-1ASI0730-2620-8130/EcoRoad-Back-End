using EcoRoad.Compliance.Domain.Model.Aggregates;
using EcoRoad.Compliance.Domain.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Compliance.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ComplianceReportRepository : IComplianceReportRepository
{
    private readonly AppDbContext _context;

    public ComplianceReportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ComplianceReport?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<ComplianceReport>()
            .AsNoTracking()
            .FirstOrDefaultAsync(report => report.Id == id, cancellationToken);
    }

    public async Task AddAsync(ComplianceReport report, CancellationToken cancellationToken = default)
    {
        await _context.Set<ComplianceReport>().AddAsync(report, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ComplianceReport report, CancellationToken cancellationToken = default)
    {
        _context.Set<ComplianceReport>().Update(report);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
