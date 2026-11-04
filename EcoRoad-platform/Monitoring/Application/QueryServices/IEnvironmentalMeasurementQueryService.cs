using EcoRoad.Monitoring.Domain.Model.Aggregates;
using EcoRoad.Monitoring.Domain.Model.Queries;

namespace EcoRoad.Monitoring.Application.QueryServices;

/// <summary>
/// Application service that answers the queries about environmental measurements.
/// </summary>
public interface IEnvironmentalMeasurementQueryService
{
    /// <summary>Retrieves a measurement.</summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The measurement when it exists; otherwise <c>null</c>.</returns>
    Task<EnvironmentalMeasurement?> Handle(GetMeasurementByIdQuery query, CancellationToken cancellationToken = default);

    /// <summary>Builds the dashboard of a project.</summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The project dashboard.</returns>
    Task<MonitoringDashboard> Handle(GetMonitoringDashboardQuery query, CancellationToken cancellationToken = default);

    /// <summary>Lists the history of a parameter.</summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The measurements, oldest first.</returns>
    Task<IReadOnlyList<EnvironmentalMeasurement>> Handle(GetMeasurementHistoryQuery query, CancellationToken cancellationToken = default);
}
