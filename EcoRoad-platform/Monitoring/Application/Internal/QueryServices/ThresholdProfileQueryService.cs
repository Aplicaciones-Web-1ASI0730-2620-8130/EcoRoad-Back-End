using EcoRoad.Monitoring.Application.QueryServices;
using EcoRoad.Monitoring.Domain.Model.Aggregates;
using EcoRoad.Monitoring.Domain.Model.Queries;
using EcoRoad.Monitoring.Domain.Repositories;

namespace EcoRoad.Monitoring.Application.Internal.QueryServices;

/// <summary>
/// Answers the queries about threshold profiles.
/// </summary>
public class ThresholdProfileQueryService : IThresholdProfileQueryService
{
    private readonly IThresholdProfileRepository _profiles;

    /// <summary>
    /// Initializes a new instance of the <see cref="ThresholdProfileQueryService" /> class.
    /// </summary>
    /// <param name="profiles">Repository of threshold profiles.</param>
    public ThresholdProfileQueryService(IThresholdProfileRepository profiles)
    {
        _profiles = profiles;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ThresholdProfile>> Handle(
        GetThresholdProfilesQuery query,
        CancellationToken cancellationToken = default)
    {
        var profiles = await _profiles.ListAsync(cancellationToken);
        return profiles.OrderBy(profile => profile.ParameterType).ToList();
    }

    /// <inheritdoc />
    public async Task<ThresholdProfile?> Handle(
        GetThresholdProfileByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _profiles.FindByIdAsync(query.ThresholdProfileId, cancellationToken);
    }
}
