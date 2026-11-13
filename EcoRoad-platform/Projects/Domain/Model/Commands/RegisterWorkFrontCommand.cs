namespace EcoRoad.Projects.Domain.Model.Commands;

public record RegisterWorkFrontCommand(
    string SectionId,
    string Name,
    string? Status);
