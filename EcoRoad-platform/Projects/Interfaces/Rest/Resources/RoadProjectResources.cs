namespace EcoRoad.Projects.Interfaces.Rest.Resources;

public record WorkFrontResource(string Id, string Name, string Status);

public record RoadSectionResource(
    string Id,
    string Name,
    double StartKilometer,
    double EndKilometer,
    IReadOnlyList<WorkFrontResource> WorkFronts);

public record RoadProjectResource(
    string Id,
    string CompanyId,
    string Name,
    string Code,
    string Status,
    DateTime CreatedAt,
    IReadOnlyList<RoadSectionResource> Sections);

public record RegisterRoadProjectResource(
    string CompanyId,
    string Name,
    string Code,
    string? Status,
    IReadOnlyList<RegisterRoadSectionResource>? Sections);

public record RegisterRoadSectionResource(
    string Name,
    double StartKilometer,
    double EndKilometer,
    IReadOnlyList<RegisterWorkFrontResource>? WorkFronts);

public record RegisterWorkFrontResource(string Name, string? Status);

public record ConfigureRoadSectionResource(
    string Name,
    double StartKilometer,
    double EndKilometer);
