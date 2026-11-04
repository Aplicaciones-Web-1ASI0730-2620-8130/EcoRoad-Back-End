using EcoRoad.Monitoring.Domain.Model.Aggregates;
using EcoRoad.Monitoring.Domain.Model.ValueObjects;
using EcoRoad.Monitoring.Domain.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
/// Entity Framework Core implementation of <see cref="IEnvironmentalMeasurementRepository" />.
/// </summary>
public class EnvironmentalMeasurementRepository
    : BaseRepository<EnvironmentalMeasurement>, IEnvironmentalMeasurementRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnvironmentalMeasurementRepository" /> class.
    /// </summary>
    /// <param name="context">The shared application database context.</param>
    public EnvironmentalMeasurementRepository(AppDbContext context)
        : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<EnvironmentalMeasurement>> ListRecentByProjectIdAsync(
        string projectId,
        DateTime since,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<EnvironmentalMeasurement>()
            .AsNoTracking()
            .Where(measurement => measurement.ProjectId == projectId && measurement.RecordedAt >= since)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<EnvironmentalMeasurement>> ListHistoryAsync(
        string projectId,
        string? sectionId,
        ParameterType parameterType,
        DateTime? from,
        DateTime? to,
        CancellationToken cancellationToken = default)
    {
        var query = Context.Set<EnvironmentalMeasurement>()
            .AsNoTracking()
            .Where(measurement => measurement.ProjectId == projectId && measurement.ParameterType == parameterType);

        if (!string.IsNullOrWhiteSpace(sectionId))
        {
            query = query.Where(measurement => measurement.SectionId == sectionId);
        }

        if (from.HasValue)
        {
            query = query.Where(measurement => measurement.RecordedAt >= from.Value);
        }

        if (to.HasValue)
        {
            query = query.Where(measurement => measurement.RecordedAt < to.Value);
        }

        return await query.OrderBy(measurement => measurement.RecordedAt).ToListAsync(cancellationToken);
    }
}
