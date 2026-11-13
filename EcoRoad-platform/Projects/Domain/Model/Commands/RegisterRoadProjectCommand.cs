namespace EcoRoad.Projects.Domain.Model.Commands;

public record WorkFrontRegistration(string Name, string? Status);

public record RoadSectionRegistration(
    string Name,
    double StartKilometer,
    double EndKilometer,
    IReadOnlyList<WorkFrontRegistration>? WorkFronts);

public record RegisterRoadProjectCommand(
    string CompanyId,
    string Name,
    string Code,
    string? Status,
    IReadOnlyList<RoadSectionRegistration>? Sections);
