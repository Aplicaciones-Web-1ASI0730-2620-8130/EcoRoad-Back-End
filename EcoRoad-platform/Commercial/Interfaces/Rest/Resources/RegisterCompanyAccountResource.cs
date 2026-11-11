using EcoRoad.Commercial.Domain.Model.ValueObjects;

namespace EcoRoad.Commercial.Interfaces.Rest.Resources;

public record RegisterCompanyAccountResource(string Name, string Code, CompanySegment Segment);
