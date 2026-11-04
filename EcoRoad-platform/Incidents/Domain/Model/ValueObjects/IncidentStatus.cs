namespace EcoRoad.Incidents.Domain.Model.ValueObjects;

/// <summary>
/// Lifecycle of an environmental incident.
/// </summary>
public enum IncidentStatus
{
    /// <summary>The incident was opened from an alert and waits for a responsible person.</summary>
    Open = 1,

    /// <summary>A responsible person is executing the remediation.</summary>
    Assigned = 2,

    /// <summary>The remediation was completed and documented with evidence.</summary>
    Attended = 3,

    /// <summary>The incident was formally closed.</summary>
    Closed = 4
}
