using EcoRoad.Projects.Domain.Model.Aggregates;
using EcoRoad.Projects.Domain.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Projects.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class RoadSectionRepository : IRoadSectionRepository
{
    private readonly AppDbContext _context;

    public RoadSectionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RoadSection?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<RoadSection>()
            .AsNoTracking()
            .FirstOrDefaultAsync(section => section.Id == id, cancellationToken);
    }

    public async Task<RoadSection?> GetByIdWithWorkFrontsAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<RoadSection>()
            .AsNoTracking()
            .Include(section => section.WorkFronts)
            .FirstOrDefaultAsync(section => section.Id == id, cancellationToken);
    }

    public async Task AddAsync(RoadSection section, CancellationToken cancellationToken = default)
    {
        await _context.Set<RoadSection>().AddAsync(section, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(RoadSection section, CancellationToken cancellationToken = default)
    {
        _context.Set<RoadSection>().Update(section);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var section = await _context.Set<RoadSection>()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (section == null)
        {
            return;
        }

        _context.Set<RoadSection>().Remove(section);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
