using EcoRoad.Assets.Domain.Model.Aggregates;
using EcoRoad.Assets.Domain.Model.ValueObjects;
using EcoRoad.Shared.Domain.Repositories;

namespace EcoRoad.Assets.Domain.Repositories;

/// <summary>
/// Repository of the <see cref="Sensor" /> aggregate.
/// </summary>
public interface ISensorRepository : IBaseRepository<Sensor>
{
    /// <summary>
    /// Checks whether a serial number is already registered.
    /// </summary>
    /// <param name="serialNumber">Normalized serial number.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns><c>true</c> when the serial number exists.</returns>
    Task<bool> ExistsBySerialNumberAsync(string serialNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists the sensor inventory with optional filters, ordered by code.
    /// </summary>
    /// <param name="projectId">Optional road project filter.</param>
    /// <param name="status">Optional deployment status filter.</param>
    /// <param name="search">Optional text matched against the code, name and serial number.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The matching sensors.</returns>
    Task<IReadOnlyList<Sensor>> ListByFiltersAsync(
        string? projectId,
        SensorStatus? status,
        string? search,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts the sensors installed at a monitoring point.
    /// </summary>
    /// <param name="monitoringPointId">Identifier of the point.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The number of installed sensors.</returns>
    Task<int> CountByMonitoringPointIdAsync(string monitoringPointId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether a point already hosts another sensor of the same type.
    /// </summary>
    /// <param name="monitoringPointId">Identifier of the point.</param>
    /// <param name="type">Sensor type to check.</param>
    /// <param name="excludedSensorId">Sensor excluded from the check.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns><c>true</c> when the type is already present at the point.</returns>
    Task<bool> ExistsTypeAtMonitoringPointAsync(
        string monitoringPointId,
        SensorType type,
        string excludedSensorId,
        CancellationToken cancellationToken = default);
}
