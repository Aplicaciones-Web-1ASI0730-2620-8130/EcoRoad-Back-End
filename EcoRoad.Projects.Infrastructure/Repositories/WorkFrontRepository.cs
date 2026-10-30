using EcoRoad.Projects.Domain.Entities;
using EcoRoad.Projects.Domain.Repositories;
using EcoRoad.Projects.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Projects.Infrastructure.Repositories;

public class WorkFrontRepository : IWorkFrontRepository
{
    private readonly ProjectDbContext _context;

    public WorkFrontRepository(ProjectDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<WorkFront>> GetBySectionIdAsync(
        string sectionId,
        CancellationToken cancellationToken = default)
    {
        return await _context.WorkFronts
            .AsNoTracking()
            .Where(workFront => workFront.SectionId == sectionId)
            .ToListAsync(cancellationToken);
    }

    public async Task<WorkFront?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.WorkFronts
            .AsNoTracking()
            .FirstOrDefaultAsync(workFront => workFront.Id == id, cancellationToken);
    }

    public async Task AddAsync(WorkFront workFront, CancellationToken cancellationToken = default)
    {
        await _context.WorkFronts.AddAsync(workFront, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(WorkFront workFront, CancellationToken cancellationToken = default)
    {
        _context.WorkFronts.Update(workFront);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var workFront = await _context.WorkFronts
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (workFront == null)
        {
            return;
        }

        _context.WorkFronts.Remove(workFront);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
