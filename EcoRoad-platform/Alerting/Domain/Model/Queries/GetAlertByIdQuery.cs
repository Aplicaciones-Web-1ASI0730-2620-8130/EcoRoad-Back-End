namespace EcoRoad.Alerting.Domain.Model.Queries;

/// <summary>
///     Reads one environmental alert by its id.
/// </summary>
public record GetAlertByIdQuery(string AlertId);
