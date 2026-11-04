namespace EcoRoad.Incidents.Domain.Model.ValueObjects;

/// <summary>
/// Severity inherited from the alert that originated the incident.
/// </summary>
public enum IncidentSeverity
{
    /// <summary>A threshold was exceeded and requires preventive action.</summary>
    Warning = 1,

    /// <summary>A critical threshold was exceeded and requires immediate action.</summary>
    Critical = 2
}
