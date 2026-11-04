using EcoRoad.Assets.Domain.Model.Aggregates;
using EcoRoad.Assets.Domain.Model.ValueObjects;
using EcoRoad.Assets.Domain.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Assets.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
/// Entity Framework Core implementation of <see cref="ISensorRepository" />.
/// </summary>
public class SensorRepository : BaseRepository<Sensor>, ISensorRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SensorRepository" /> class.
    /// </summary>
    /// <param name="context">The shared application database context.</param>
    public SensorRepository(AppDbContext context)
        : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<bool> ExistsBySerialNumberAsync(string serialNumber, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Sensor>().AnyAsync(sensor => sensor.SerialNumber == serialNumber, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Sensor>> ListByFiltersAsync(
        string? projectId,
        SensorStatus? status,
        string? search,
        CancellationToken cancellationToken = default)
    {
        var query = Context.Set<Sensor>().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(projectId))
        {
            query = query.Where(sensor => sensor.ProjectId == projectId);
        }

        if (status.HasValue)
        {
            query = query.Where(sensor => sensor.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(sensor => sensor.Code.Contains(term)
                || sensor.Name.Contains(term)
                || sensor.SerialNumber.Contains(term));
        }

        return await query.OrderBy(sensor => sensor.Code).ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CountByMonitoringPointIdAsync(
        string monitoringPointId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Sensor>()
            .CountAsync(sensor => sensor.MonitoringPointId == monitoringPointId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsTypeAtMonitoringPointAsync(
        string monitoringPointId,
        SensorType type,
        string excludedSensorId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Sensor>().AnyAsync(
            sensor => sensor.MonitoringPointId == monitoringPointId
                && sensor.Type == type
                && sensor.Id != excludedSensorId,
            cancellationToken);
    }
}
