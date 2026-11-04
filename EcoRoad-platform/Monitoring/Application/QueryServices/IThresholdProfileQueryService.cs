using EcoRoad.Monitoring.Domain.Model.Aggregates;
using EcoRoad.Monitoring.Domain.Model.Queries;

namespace EcoRoad.Monitoring.Application.QueryServices;

/// <summary>
/// Application service that answers the queries about threshold profiles.
/// </summary>
public interface IThresholdProfileQueryService
{
    /// <summary>Lists every threshold profile.</summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The profiles ordered by parameter.</returns>
    Task<IReadOnlyList<ThresholdProfile>> Handle(GetThresholdProfilesQuery query, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a threshold profile.</summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The profile when it exists; otherwise <c>null</c>.</returns>
    Task<ThresholdProfile?> Handle(GetThresholdProfileByIdQuery query, CancellationToken cancellationToken = default);
}
