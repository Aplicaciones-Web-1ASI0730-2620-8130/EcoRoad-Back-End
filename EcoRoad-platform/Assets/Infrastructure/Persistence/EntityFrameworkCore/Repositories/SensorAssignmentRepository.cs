using EcoRoad.Assets.Domain.Model.Aggregates;
using EcoRoad.Assets.Domain.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Assets.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
/// Entity Framework Core implementation of <see cref="ISensorAssignmentRepository" />.
/// </summary>
public class SensorAssignmentRepository : BaseRepository<SensorAssignment>, ISensorAssignmentRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SensorAssignmentRepository" /> class.
    /// </summary>
    /// <param name="context">The shared application database context.</param>
    public SensorAssignmentRepository(AppDbContext context)
        : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<SensorAssignment?> FindActiveBySensorIdAsync(
        string sensorId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<SensorAssignment>()
            .FirstOrDefaultAsync(
                assignment => assignment.SensorId == sensorId && assignment.ReleasedAt == null,
                cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<SensorAssignment>> ListBySensorIdAsync(
        string sensorId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<SensorAssignment>()
            .AsNoTracking()
            .Where(assignment => assignment.SensorId == sensorId)
            .OrderByDescending(assignment => assignment.AssignedAt)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByMonitoringPointIdAsync(
        string monitoringPointId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<SensorAssignment>()
            .AnyAsync(assignment => assignment.MonitoringPointId == monitoringPointId, cancellationToken);
    }
}
