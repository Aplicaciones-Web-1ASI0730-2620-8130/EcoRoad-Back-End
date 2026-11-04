using EcoRoad.Monitoring.Domain.Model.Aggregates;
using EcoRoad.Monitoring.Domain.Model.ValueObjects;
using EcoRoad.Monitoring.Domain.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
/// Entity Framework Core implementation of <see cref="IThresholdProfileRepository" />.
/// </summary>
public class ThresholdProfileRepository : BaseRepository<ThresholdProfile>, IThresholdProfileRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ThresholdProfileRepository" /> class.
    /// </summary>
    /// <param name="context">The shared application database context.</param>
    public ThresholdProfileRepository(AppDbContext context)
        : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<ThresholdProfile?> FindByParameterTypeAsync(
        ParameterType parameterType,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<ThresholdProfile>()
            .FirstOrDefaultAsync(profile => profile.ParameterType == parameterType, cancellationToken);
    }
}
