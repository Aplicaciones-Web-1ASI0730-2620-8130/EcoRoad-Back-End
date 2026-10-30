using EcoRoad.Projects.Domain.Entities;
using EcoRoad.Projects.Domain.Repositories;
using EcoRoad.Projects.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Projects.Infrastructure.Repositories;

public class RoadSectionRepository : IRoadSectionRepository
{
    private readonly ProjectDbContext _context;

    public RoadSectionRepository(ProjectDbContext context)
    {
        _context = context;
    }

    public async Task<RoadSection?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.RoadSections
            .AsNoTracking()
            .FirstOrDefaultAsync(section => section.Id == id, cancellationToken);
    }

    public async Task<RoadSection?> GetByIdWithWorkFrontsAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.RoadSections
            .AsNoTracking()
            .Include(section => section.WorkFronts)
            .FirstOrDefaultAsync(section => section.Id == id, cancellationToken);
    }

    public async Task AddAsync(RoadSection section, CancellationToken cancellationToken = default)
    {
        await _context.RoadSections.AddAsync(section, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(RoadSection section, CancellationToken cancellationToken = default)
    {
        _context.RoadSections.Update(section);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var section = await _context.RoadSections
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (section == null)
        {
            return;
        }

        _context.RoadSections.Remove(section);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
