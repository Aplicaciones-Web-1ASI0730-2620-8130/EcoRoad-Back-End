using EcoRoad.Alerting.Domain.Model.Aggregates;

namespace EcoRoad.Alerting.Domain.Repositories;

/// <summary>
///     Repository contract for the DeviationAssessment aggregate.
/// </summary>
public interface IDeviationAssessmentRepository
{
    /// <summary>
    ///     Finds a deviation assessment by its id.
    /// </summary>
    /// <param name="assessmentId">The assessment id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The assessment when it exists; otherwise null.</returns>
    Task<DeviationAssessment?> FindByIdAsync(string assessmentId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Lists deviation assessments, optionally limited to one measurement.
    /// </summary>
    /// <param name="measurementId">The measurement id, or null for every measurement.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The matching assessments, newest first.</returns>
    Task<IReadOnlyList<DeviationAssessment>> ListAsync(
        string? measurementId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Adds a deviation assessment to the repository.
    /// </summary>
    /// <param name="assessment">The assessment to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task AddAsync(DeviationAssessment assessment, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates a deviation assessment already stored in the repository.
    /// </summary>
    /// <param name="assessment">The assessment to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task UpdateAsync(DeviationAssessment assessment, CancellationToken cancellationToken = default);
}
