using EcoRoad.Assets.Application.QueryServices;
using EcoRoad.Assets.Domain.Model.Aggregates;
using EcoRoad.Assets.Domain.Model.Queries;
using EcoRoad.Assets.Domain.Model.ValueObjects;
using EcoRoad.Assets.Domain.Repositories;

namespace EcoRoad.Assets.Application.Internal.QueryServices;

/// <summary>
/// Answers the queries about sensors and the fleet indicators.
/// </summary>
public class SensorQueryService : ISensorQueryService
{
    private readonly ISensorRepository _sensors;
    private readonly ISensorAssignmentRepository _assignments;

    /// <summary>
    /// Initializes a new instance of the <see cref="SensorQueryService" /> class.
    /// </summary>
    /// <param name="sensors">Repository of sensors.</param>
    /// <param name="assignments">Repository of the assignment history.</param>
    public SensorQueryService(ISensorRepository sensors, ISensorAssignmentRepository assignments)
    {
        _sensors = sensors;
        _assignments = assignments;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Sensor>> Handle(GetSensorsQuery query, CancellationToken cancellationToken = default)
    {
        return await _sensors.ListByFiltersAsync(query.ProjectId, query.Status, query.Search, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Sensor?> Handle(GetSensorByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _sensors.FindByIdAsync(query.SensorId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<SensorAssignment>> Handle(
        GetSensorAssignmentsQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _assignments.ListBySensorIdAsync(query.SensorId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<AssetSummary> Handle(GetAssetSummaryQuery query, CancellationToken cancellationToken = default)
    {
        var sensors = await _sensors.ListByFiltersAsync(query.ProjectId, null, null, cancellationToken);
        var now = DateTime.UtcNow;

        return new AssetSummary(
            Total: sensors.Count,
            ReadyToReport: sensors.Count(sensor => sensor.CanReport(now)),
            Assigned: sensors.Count(sensor => sensor.Status == SensorStatus.Assigned),
            Available: sensors.Count(sensor => sensor.Status == SensorStatus.Available),
            CalibrationDue: sensors.Count(sensor => sensor.MonitoringPointId != null && !sensor.IsCalibrationValid(now)));
    }
}
