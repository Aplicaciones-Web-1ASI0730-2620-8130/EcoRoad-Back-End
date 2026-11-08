using EcoRoad.Alerting.Domain.Model.Aggregates;
using EcoRoad.Alerting.Domain.Model.ValueObjects;

namespace EcoRoad.Alerting.Domain.Repositories;

/// <summary>
///     Repository contract for the Alert aggregate.
/// </summary>
public interface IAlertRepository
{
    /// <summary>
    ///     Finds an alert by its id.
    /// </summary>
    /// <param name="alertId">The alert id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The alert when it exists; otherwise null.</returns>
    Task<Alert?> FindByIdAsync(string alertId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Lists alerts that are still raised.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The raised alerts, newest first.</returns>
    Task<IReadOnlyList<Alert>> ListActiveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Lists alerts filtered by monitoring point and status. A null filter is ignored.
    /// </summary>
    /// <param name="monitoringPointId">The monitoring point id, or null for every point.</param>
    /// <param name="status">The alert status, or null for every status.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The matching alerts, newest first.</returns>
    Task<IReadOnlyList<Alert>> ListAsync(
        string? monitoringPointId,
        AlertStatus? status,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Adds an alert to the repository.
    /// </summary>
    /// <param name="alert">The alert to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task AddAsync(Alert alert, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates an alert already stored in the repository.
    /// </summary>
    /// <param name="alert">The alert to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task UpdateAsync(Alert alert, CancellationToken cancellationToken = default);
}
