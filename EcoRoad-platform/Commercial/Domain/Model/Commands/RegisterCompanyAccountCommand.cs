using EcoRoad.Commercial.Domain.Model.ValueObjects;

namespace EcoRoad.Commercial.Domain.Model.Commands;

public record RegisterCompanyAccountCommand(string Name, string Code, CompanySegment Segment);
