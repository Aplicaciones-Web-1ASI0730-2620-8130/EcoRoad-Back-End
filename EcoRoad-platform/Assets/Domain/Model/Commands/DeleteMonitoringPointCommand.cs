namespace EcoRoad.Assets.Domain.Model.Commands;

/// <summary>
/// Deletes a monitoring point that has no sensors installed.
/// </summary>
/// <param name="MonitoringPointId">Identifier of the point.</param>
public record DeleteMonitoringPointCommand(string MonitoringPointId);
