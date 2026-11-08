namespace EcoRoad.Alerting.Interfaces.Rest.Resources;

/// <summary>
///     Request body used to acknowledge an alert.
/// </summary>
public record AcknowledgeAlertResource(string UserId);
