namespace EcoRoad.Assets.Domain.Model.Queries;

/// <summary>
/// Retrieves a monitoring point by its identifier.
/// </summary>
/// <param name="MonitoringPointId">Identifier of the point.</param>
public record GetMonitoringPointByIdQuery(string MonitoringPointId);
