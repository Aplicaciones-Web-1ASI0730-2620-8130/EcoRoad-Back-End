using EcoRoad.Projects.Domain.Model.Aggregates;
using EcoRoad.Projects.Domain.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Projects.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class WorkFrontRepository : IWorkFrontRepository
{
    private readonly AppDbContext _context;

    public WorkFrontRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<WorkFront>> GetBySectionIdAsync(
        string sectionId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<WorkFront>()
            .AsNoTracking()
            .Where(workFront => workFront.SectionId == sectionId)
            .ToListAsync(cancellationToken);
    }

    public async Task<WorkFront?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<WorkFront>()
            .AsNoTracking()
            .FirstOrDefaultAsync(workFront => workFront.Id == id, cancellationToken);
    }

    public async Task AddAsync(WorkFront workFront, CancellationToken cancellationToken = default)
    {
        await _context.Set<WorkFront>().AddAsync(workFront, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(WorkFront workFront, CancellationToken cancellationToken = default)
    {
        _context.Set<WorkFront>().Update(workFront);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var workFront = await _context.Set<WorkFront>()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (workFront == null)
        {
            return;
        }

        _context.Set<WorkFront>().Remove(workFront);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
