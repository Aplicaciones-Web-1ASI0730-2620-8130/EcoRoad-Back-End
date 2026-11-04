using EcoRoad.Incidents.Domain.Model.ValueObjects;

namespace EcoRoad.Incidents.Domain.Model.Commands;

/// <summary>
/// Opens an environmental incident from an alert.
/// </summary>
/// <param name="CompanyId">Identifier of the company.</param>
/// <param name="AlertId">Identifier of the originating alert; an alert can open only one incident.</param>
/// <param name="ProjectId">Identifier of the road project.</param>
/// <param name="SectionId">Identifier of the road section.</param>
/// <param name="Title">Short title, usually the affected indicator.</param>
/// <param name="Description">Description of the observed environmental impact.</param>
/// <param name="Severity">Severity inherited from the alert.</param>
/// <param name="OpenedBy">Identifier of the user that opens the incident.</param>
public record OpenIncidentCommand(
    string CompanyId,
    string AlertId,
    string ProjectId,
    string SectionId,
    string Title,
    string Description,
    IncidentSeverity Severity,
    string OpenedBy);
