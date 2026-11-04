namespace EcoRoad.Incidents.Domain.Model.ValueObjects;

/// <summary>
/// Lifecycle of a corrective action.
/// </summary>
public enum ActionStatus
{
    /// <summary>The action was registered and has not started.</summary>
    Assigned = 1,

    /// <summary>The field crew is executing the action.</summary>
    InProgress = 2,

    /// <summary>The action was completed.</summary>
    Executed = 3
}
