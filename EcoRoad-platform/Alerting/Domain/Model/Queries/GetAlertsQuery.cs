using EcoRoad.Alerting.Domain.Model.ValueObjects;

namespace EcoRoad.Alerting.Domain.Model.Queries;

/// <summary>
///     Reads alerts filtered by monitoring point and status.
///     A null filter is ignored.
/// </summary>
public record GetAlertsQuery(string? MonitoringPointId, AlertStatus? Status);
