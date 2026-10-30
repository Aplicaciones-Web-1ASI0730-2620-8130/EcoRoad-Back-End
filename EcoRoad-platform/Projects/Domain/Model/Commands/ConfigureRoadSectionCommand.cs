namespace EcoRoad.Projects.Domain.Model.Commands;

public record ConfigureRoadSectionCommand(
    string ProjectId,
    string Name,
    double StartKilometer,
    double EndKilometer);
