using EcoRoad.Alerting.Domain.Model.Queries;

namespace EcoRoad.Alerting.Application.QueryServices;

/// <summary>
///     Query service for alerts and deviation assessments.
/// </summary>
public interface IAlertingQueryService
{
    /// <summary>
    ///     Returns one alert, or null when it does not exist.
    /// </summary>
    Task<ActiveAlertView?> Handle(GetAlertByIdQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Returns alerts filtered by monitoring point and status.
    /// </summary>
    Task<IReadOnlyList<ActiveAlertView>> Handle(GetAlertsQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Returns alerts that are still raised.
    /// </summary>
    Task<IReadOnlyList<ActiveAlertView>> Handle(
        GetActiveAlertsQuery query,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Returns one deviation assessment, or null when it does not exist.
    /// </summary>
    Task<DeviationAssessmentView?> Handle(
        GetDeviationAssessmentByIdQuery query,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Returns deviation assessments, optionally limited to one measurement.
    /// </summary>
    Task<IReadOnlyList<DeviationAssessmentView>> Handle(
        GetDeviationAssessmentsQuery query,
        CancellationToken cancellationToken = default);
}
