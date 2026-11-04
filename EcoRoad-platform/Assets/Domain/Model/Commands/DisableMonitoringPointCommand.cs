namespace EcoRoad.Assets.Domain.Model.Commands;

/// <summary>
/// Takes a monitoring point without sensors out of service.
/// </summary>
/// <param name="MonitoringPointId">Identifier of the point.</param>
public record DisableMonitoringPointCommand(string MonitoringPointId);
