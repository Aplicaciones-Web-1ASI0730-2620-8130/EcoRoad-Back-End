namespace EcoRoad.Alerting.Domain.Model.ValueObjects;

/// <summary>
///     Lifecycle status of an environmental alert.
/// </summary>
public enum AlertStatus
{
    Raised = 1,
    Acknowledged = 2,
    Closed = 3
}
