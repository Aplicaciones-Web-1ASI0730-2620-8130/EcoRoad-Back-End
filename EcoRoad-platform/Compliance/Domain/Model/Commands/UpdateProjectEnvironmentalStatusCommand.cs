using EcoRoad.Compliance.Domain.Model.ValueObjects;

namespace EcoRoad.Compliance.Domain.Model.Commands;

public record UpdateProjectEnvironmentalStatusCommand(string ProjectId, EnvironmentalStanding Standing);
