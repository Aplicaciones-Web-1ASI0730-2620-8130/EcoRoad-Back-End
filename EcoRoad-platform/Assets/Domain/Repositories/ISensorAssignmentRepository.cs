using EcoRoad.Assets.Domain.Model.Aggregates;
using EcoRoad.Shared.Domain.Repositories;

namespace EcoRoad.Assets.Domain.Repositories;

/// <summary>
/// Repository of the <see cref="SensorAssignment" /> history.
/// </summary>
public interface ISensorAssignmentRepository : IBaseRepository<SensorAssignment>
{
    /// <summary>
    /// Finds the open assignment of a sensor.
    /// </summary>
    /// <param name="sensorId">Identifier of the sensor.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The open assignment, or <c>null</c> when the sensor is not installed.</returns>
    Task<SensorAssignment?> FindActiveBySensorIdAsync(string sensorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists the installation history of a sensor, newest first.
    /// </summary>
    /// <param name="sensorId">Identifier of the sensor.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The assignments of the sensor.</returns>
    Task<IReadOnlyList<SensorAssignment>> ListBySensorIdAsync(string sensorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether a monitoring point ever hosted a sensor.
    /// </summary>
    /// <param name="monitoringPointId">Identifier of the point.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns><c>true</c> when the point has deployment history.</returns>
    Task<bool> ExistsByMonitoringPointIdAsync(string monitoringPointId, CancellationToken cancellationToken = default);
}
