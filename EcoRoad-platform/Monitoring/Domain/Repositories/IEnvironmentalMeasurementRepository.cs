using EcoRoad.Monitoring.Domain.Model.Aggregates;
using EcoRoad.Monitoring.Domain.Model.ValueObjects;
using EcoRoad.Shared.Domain.Repositories;

namespace EcoRoad.Monitoring.Domain.Repositories;

/// <summary>
/// Repository of the <see cref="EnvironmentalMeasurement" /> aggregate.
/// </summary>
public interface IEnvironmentalMeasurementRepository : IBaseRepository<EnvironmentalMeasurement>
{
    /// <summary>
    /// Lists the measurements of a project recorded since a date.
    /// </summary>
    /// <param name="projectId">Identifier of the road project.</param>
    /// <param name="since">UTC date from which measurements are returned.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The recent measurements of the project.</returns>
    Task<IReadOnlyList<EnvironmentalMeasurement>> ListRecentByProjectIdAsync(
        string projectId,
        DateTime since,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists the measurements of a parameter in a project, oldest first.
    /// </summary>
    /// <param name="projectId">Identifier of the road project.</param>
    /// <param name="sectionId">Optional road section filter.</param>
    /// <param name="parameterType">The environmental parameter.</param>
    /// <param name="from">Optional UTC lower bound, inclusive.</param>
    /// <param name="to">Optional UTC upper bound, exclusive.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The matching measurements.</returns>
    Task<IReadOnlyList<EnvironmentalMeasurement>> ListHistoryAsync(
        string projectId,
        string? sectionId,
        ParameterType parameterType,
        DateTime? from,
        DateTime? to,
        CancellationToken cancellationToken = default);
}
