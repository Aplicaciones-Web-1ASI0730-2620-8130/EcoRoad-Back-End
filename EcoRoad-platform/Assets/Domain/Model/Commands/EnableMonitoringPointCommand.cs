namespace EcoRoad.Assets.Domain.Model.Commands;

/// <summary>
/// Puts a disabled monitoring point back into service.
/// </summary>
/// <param name="MonitoringPointId">Identifier of the point.</param>
public record EnableMonitoringPointCommand(string MonitoringPointId);
