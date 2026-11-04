using EcoRoad.Assets.Domain.Model.Aggregates;
using EcoRoad.Assets.Domain.Model.Queries;

namespace EcoRoad.Assets.Application.QueryServices;

/// <summary>
/// Application service that answers the queries about monitoring points.
/// </summary>
public interface IMonitoringPointQueryService
{
    /// <summary>Lists monitoring points.</summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The matching points.</returns>
    Task<IReadOnlyList<MonitoringPoint>> Handle(GetMonitoringPointsQuery query, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a monitoring point.</summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The point when it exists; otherwise <c>null</c>.</returns>
    Task<MonitoringPoint?> Handle(GetMonitoringPointByIdQuery query, CancellationToken cancellationToken = default);
}
