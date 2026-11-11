using EcoRoad.Commercial.Domain.Model.ValueObjects;

namespace EcoRoad.Commercial.Interfaces.Rest.Resources;

public record CompanyAccountResource(
    string Id,
    string Name,
    string Code,
    CompanySegment Segment,
    CompanyAccountStatus Status,
    DateTime CreatedAt,
    bool HasPlatformAccess);
