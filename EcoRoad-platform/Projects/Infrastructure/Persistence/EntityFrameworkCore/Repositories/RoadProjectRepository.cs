using EcoRoad.Projects.Domain.Model.Aggregates;
using EcoRoad.Projects.Domain.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Projects.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class RoadProjectRepository : IRoadProjectRepository
{
    private readonly AppDbContext _context;

    public RoadProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<RoadProject>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<RoadProject>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<RoadProject?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<RoadProject>()
            .AsNoTracking()
            .FirstOrDefaultAsync(project => project.Id == id, cancellationToken);
    }

    public async Task<RoadProject?> GetByIdWithSectionsAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<RoadProject>()
            .AsNoTracking()
            .Include(project => project.Sections)
            .ThenInclude(section => section.WorkFronts)
            .FirstOrDefaultAsync(project => project.Id == id, cancellationToken);
    }

    public async Task AddAsync(RoadProject project, CancellationToken cancellationToken = default)
    {
        await _context.Set<RoadProject>().AddAsync(project, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(RoadProject project, CancellationToken cancellationToken = default)
    {
        _context.Set<RoadProject>().Update(project);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var project = await _context.Set<RoadProject>()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (project == null)
        {
            return;
        }

        _context.Set<RoadProject>().Remove(project);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
