using EcoRoad.Commercial.Domain.Model.Aggregates;
using EcoRoad.Commercial.Domain.Model.Commands;
using EcoRoad.Commercial.Domain.Model.Queries;
using EcoRoad.Commercial.Interfaces.Rest.Resources;

namespace EcoRoad.Commercial.Interfaces.Rest.Transform;

public static class RegisterCompanyAccountCommandFromResourceAssembler
{
    public static RegisterCompanyAccountCommand ToCommandFromResource(RegisterCompanyAccountResource resource)
    {
        return new RegisterCompanyAccountCommand(resource.Name, resource.Code, resource.Segment);
    }
}
