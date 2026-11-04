using EcoRoad.Assets.Domain.Model.Aggregates;
using EcoRoad.Assets.Domain.Model.Queries;

namespace EcoRoad.Assets.Application.QueryServices;

/// <summary>
/// Application service that answers the queries about sensors and the fleet.
/// </summary>
public interface ISensorQueryService
{
    /// <summary>Lists the sensor inventory.</summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The matching sensors.</returns>
    Task<IReadOnlyList<Sensor>> Handle(GetSensorsQuery query, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a sensor.</summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The sensor when it exists; otherwise <c>null</c>.</returns>
    Task<Sensor?> Handle(GetSensorByIdQuery query, CancellationToken cancellationToken = default);

    /// <summary>Lists the installation history of a sensor.</summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The assignments, newest first.</returns>
    Task<IReadOnlyList<SensorAssignment>> Handle(GetSensorAssignmentsQuery query, CancellationToken cancellationToken = default);

    /// <summary>Computes the deployment indicators of the fleet.</summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The fleet summary.</returns>
    Task<AssetSummary> Handle(GetAssetSummaryQuery query, CancellationToken cancellationToken = default);
}
