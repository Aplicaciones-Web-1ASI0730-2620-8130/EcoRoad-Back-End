using EcoRoad.Compliance.Domain.Model.ValueObjects;

namespace EcoRoad.Compliance.Domain.Model.Commands;

public record UpdatePortfolioEnvironmentalStatusCommand(string PortfolioId, EnvironmentalStanding Standing);
