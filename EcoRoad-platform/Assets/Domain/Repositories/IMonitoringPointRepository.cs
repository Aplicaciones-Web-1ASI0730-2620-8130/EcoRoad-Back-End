using EcoRoad.Assets.Domain.Model.Aggregates;
using EcoRoad.Shared.Domain.Repositories;

namespace EcoRoad.Assets.Domain.Repositories;

/// <summary>
/// Repository of the <see cref="MonitoringPoint" /> aggregate.
/// </summary>
public interface IMonitoringPointRepository : IBaseRepository<MonitoringPoint>
{
    /// <summary>
    /// Lists monitoring points ordered by kilometer post, optionally limited to a project.
    /// </summary>
    /// <param name="projectId">Optional road project filter.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The matching points.</returns>
    Task<IReadOnlyList<MonitoringPoint>> ListByProjectIdAsync(string? projectId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether another point already uses a name inside the same section.
    /// </summary>
    /// <param name="sectionId">Identifier of the road section.</param>
    /// <param name="name">Name of the point.</param>
    /// <param name="excludedPointId">Point excluded from the check, used when updating.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns><c>true</c> when the name is already taken.</returns>
    Task<bool> ExistsByNameInSectionAsync(
        string sectionId,
        string name,
        string? excludedPointId,
        CancellationToken cancellationToken = default);
}
