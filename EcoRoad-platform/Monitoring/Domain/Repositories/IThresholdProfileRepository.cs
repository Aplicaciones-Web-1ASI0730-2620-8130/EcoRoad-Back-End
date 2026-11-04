using EcoRoad.Monitoring.Domain.Model.Aggregates;
using EcoRoad.Monitoring.Domain.Model.ValueObjects;
using EcoRoad.Shared.Domain.Repositories;

namespace EcoRoad.Monitoring.Domain.Repositories;

/// <summary>
/// Repository of the <see cref="ThresholdProfile" /> aggregate.
/// </summary>
public interface IThresholdProfileRepository : IBaseRepository<ThresholdProfile>
{
    /// <summary>
    /// Finds the threshold profile of an environmental parameter.
    /// </summary>
    /// <param name="parameterType">The environmental parameter.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The profile when it exists; otherwise <c>null</c>.</returns>
    Task<ThresholdProfile?> FindByParameterTypeAsync(ParameterType parameterType, CancellationToken cancellationToken = default);
}
