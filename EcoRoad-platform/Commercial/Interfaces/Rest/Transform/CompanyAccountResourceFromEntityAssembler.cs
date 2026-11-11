using EcoRoad.Commercial.Domain.Model.Aggregates;
using EcoRoad.Commercial.Domain.Model.Commands;
using EcoRoad.Commercial.Domain.Model.Queries;
using EcoRoad.Commercial.Interfaces.Rest.Resources;

namespace EcoRoad.Commercial.Interfaces.Rest.Transform;

public static class CompanyAccountResourceFromEntityAssembler
{
    public static CompanyAccountResource ToResourceFromEntity(CompanyAccount account)
    {
        return new CompanyAccountResource(
            account.Id,
            account.Name,
            account.Code,
            account.Segment,
            account.Status,
            account.CreatedAt,
            account.HasPlatformAccess);
    }
}
