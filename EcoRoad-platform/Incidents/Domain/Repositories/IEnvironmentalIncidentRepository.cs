using EcoRoad.Incidents.Domain.Model.Aggregates;
using EcoRoad.Incidents.Domain.Model.ValueObjects;
using EcoRoad.Shared.Domain.Repositories;

namespace EcoRoad.Incidents.Domain.Repositories;

/// <summary>
/// Repository of the <see cref="EnvironmentalIncident" /> aggregate.
/// </summary>
/// <remarks>
/// <see cref="IBaseRepository{TEntity}.FindByIdAsync" /> loads the whole aggregate, including its
/// corrective actions and field evidence.
/// </remarks>
public interface IEnvironmentalIncidentRepository : IBaseRepository<EnvironmentalIncident>
{
    /// <summary>
    /// Checks whether an alert already originated an incident.
    /// </summary>
    /// <param name="alertId">Identifier of the alert.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns><c>true</c> when an incident references the alert.</returns>
    Task<bool> ExistsByAlertIdAsync(string alertId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists incidents with optional filters, ordered by status and newest first.
    /// </summary>
    /// <param name="companyId">Optional company filter.</param>
    /// <param name="status">Optional status filter.</param>
    /// <param name="projectId">Optional road project filter.</param>
    /// <param name="search">Optional text matched against the code and the title.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The matching incidents with their actions and evidence.</returns>
    Task<IReadOnlyList<EnvironmentalIncident>> ListByFiltersAsync(
        string? companyId,
        IncidentStatus? status,
        string? projectId,
        string? search,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts the incidents of every status.
    /// </summary>
    /// <param name="companyId">Optional company filter.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The number of incidents per status.</returns>
    Task<IReadOnlyDictionary<IncidentStatus, int>> CountByStatusAsync(
        string? companyId,
        CancellationToken cancellationToken = default);
}
