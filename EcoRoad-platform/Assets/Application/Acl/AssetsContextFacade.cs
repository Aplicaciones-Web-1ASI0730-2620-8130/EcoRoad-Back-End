using EcoRoad.Assets.Application.QueryServices;
using EcoRoad.Assets.Domain.Model.Queries;
using EcoRoad.Assets.Interfaces.Acl;

namespace EcoRoad.Assets.Application.Acl;

/// <summary>
/// Default implementation of <see cref="IAssetsContextFacade" /> backed by the sensor query service.
/// </summary>
public class AssetsContextFacade : IAssetsContextFacade
{
    private readonly ISensorQueryService _sensorQueries;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssetsContextFacade" /> class.
    /// </summary>
    /// <param name="sensorQueries">Query service of sensors.</param>
    public AssetsContextFacade(ISensorQueryService sensorQueries)
    {
        _sensorQueries = sensorQueries;
    }

    /// <inheritdoc />
    public async Task<bool> CanSensorReportAsync(string sensorId, CancellationToken cancellationToken = default)
    {
        var sensor = await _sensorQueries.Handle(new GetSensorByIdQuery(sensorId), cancellationToken);
        return sensor != null && sensor.CanReport(DateTime.UtcNow);
    }

    /// <inheritdoc />
    public async Task<string> FetchMonitoringPointIdBySensorIdAsync(
        string sensorId,
        CancellationToken cancellationToken = default)
    {
        var sensor = await _sensorQueries.Handle(new GetSensorByIdQuery(sensorId), cancellationToken);
        return sensor?.MonitoringPointId ?? string.Empty;
    }
}
