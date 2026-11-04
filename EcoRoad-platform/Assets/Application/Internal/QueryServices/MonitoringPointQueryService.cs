using EcoRoad.Assets.Application.QueryServices;
using EcoRoad.Assets.Domain.Model.Aggregates;
using EcoRoad.Assets.Domain.Model.Queries;
using EcoRoad.Assets.Domain.Repositories;

namespace EcoRoad.Assets.Application.Internal.QueryServices;

/// <summary>
/// Answers the queries about monitoring points.
/// </summary>
public class MonitoringPointQueryService : IMonitoringPointQueryService
{
    private readonly IMonitoringPointRepository _points;

    /// <summary>
    /// Initializes a new instance of the <see cref="MonitoringPointQueryService" /> class.
    /// </summary>
    /// <param name="points">Repository of monitoring points.</param>
    public MonitoringPointQueryService(IMonitoringPointRepository points)
    {
        _points = points;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<MonitoringPoint>> Handle(
        GetMonitoringPointsQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _points.ListByProjectIdAsync(query.ProjectId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<MonitoringPoint?> Handle(
        GetMonitoringPointByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _points.FindByIdAsync(query.MonitoringPointId, cancellationToken);
    }
}
