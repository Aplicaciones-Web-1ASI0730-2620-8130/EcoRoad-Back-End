using EcoRoad.Projects.Domain.Entities;
using EcoRoad.Projects.Domain.Repositories;
using EcoRoad.Projects.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Projects.Infrastructure.Repositories;

public class RoadProjectRepository : IRoadProjectRepository
{
    private readonly ProjectDbContext _context;

    public RoadProjectRepository(ProjectDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<RoadProject>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.RoadProjects
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<RoadProject?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.RoadProjects
            .AsNoTracking()
            .FirstOrDefaultAsync(project => project.Id == id, cancellationToken);
    }

    public async Task<RoadProject?> GetByIdWithSectionsAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.RoadProjects
            .AsNoTracking()
            .Include(project => project.Sections)
            .FirstOrDefaultAsync(project => project.Id == id, cancellationToken);
    }

    public async Task AddAsync(RoadProject project, CancellationToken cancellationToken = default)
    {
        await _context.RoadProjects.AddAsync(project, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(RoadProject project, CancellationToken cancellationToken = default)
    {
        _context.RoadProjects.Update(project);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var project = await _context.RoadProjects
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (project == null)
        {
            return;
        }

        _context.RoadProjects.Remove(project);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
