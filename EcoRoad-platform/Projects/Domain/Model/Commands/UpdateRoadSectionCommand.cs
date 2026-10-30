namespace EcoRoad.Projects.Domain.Model.Commands;

public record UpdateRoadSectionCommand(
    string ProjectId,
    string SectionId,
    string Name,
    double StartKilometer,
    double EndKilometer);
