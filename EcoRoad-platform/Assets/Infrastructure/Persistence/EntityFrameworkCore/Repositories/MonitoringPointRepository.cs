using EcoRoad.Assets.Domain.Model.Aggregates;
using EcoRoad.Assets.Domain.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Assets.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
/// Entity Framework Core implementation of <see cref="IMonitoringPointRepository" />.
/// </summary>
public class MonitoringPointRepository : BaseRepository<MonitoringPoint>, IMonitoringPointRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MonitoringPointRepository" /> class.
    /// </summary>
    /// <param name="context">The shared application database context.</param>
    public MonitoringPointRepository(AppDbContext context)
        : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<MonitoringPoint>> ListByProjectIdAsync(
        string? projectId,
        CancellationToken cancellationToken = default)
    {
        var query = Context.Set<MonitoringPoint>().AsNoTracking();
        if (!string.IsNullOrWhiteSpace(projectId))
        {
            query = query.Where(point => point.ProjectId == projectId);
        }

        return await query
            .OrderBy(point => point.ProjectId)
            .ThenBy(point => point.KilometerPost)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByNameInSectionAsync(
        string sectionId,
        string name,
        string? excludedPointId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<MonitoringPoint>().AnyAsync(
            point => point.SectionId == sectionId && point.Name == name && point.Id != excludedPointId,
            cancellationToken);
    }
}
