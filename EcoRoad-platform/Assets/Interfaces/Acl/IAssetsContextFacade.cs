namespace EcoRoad.Assets.Interfaces.Acl;

/// <summary>
/// Anti-corruption layer that lets other bounded contexts, such as Environmental Monitoring,
/// verify sensors without depending on the asset domain model.
/// </summary>
public interface IAssetsContextFacade
{
    /// <summary>
    /// Checks the reporting policy of a sensor.
    /// </summary>
    /// <param name="sensorId">Identifier of the sensor.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns><c>true</c> when the sensor is active, installed and calibrated.</returns>
    Task<bool> CanSensorReportAsync(string sensorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the monitoring point where a sensor is installed.
    /// </summary>
    /// <param name="sensorId">Identifier of the sensor.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The identifier of the point, or an empty string when the sensor is not installed.</returns>
    Task<string> FetchMonitoringPointIdBySensorIdAsync(string sensorId, CancellationToken cancellationToken = default);
}
