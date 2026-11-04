namespace EcoRoad.Incidents.Domain.Model.Commands;

/// <summary>
/// Marks an incident as attended once its remediation is complete.
/// </summary>
/// <param name="IncidentId">Identifier of the incident.</param>
public record MarkIncidentAsAttendedCommand(
    string IncidentId);
