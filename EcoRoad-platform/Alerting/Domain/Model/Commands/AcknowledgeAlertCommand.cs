namespace EcoRoad.Alerting.Domain.Model.Commands;

/// <summary>
///     Acknowledges an alert on behalf of a user.
/// </summary>
public record AcknowledgeAlertCommand(string AlertId, string UserId);
