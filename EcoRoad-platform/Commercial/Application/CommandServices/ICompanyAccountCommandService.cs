using EcoRoad.Commercial.Domain.Model.Aggregates;
using EcoRoad.Commercial.Domain.Model.Commands;

namespace EcoRoad.Commercial.Application.CommandServices;

public interface ICompanyAccountCommandService
{
    Task<CompanyAccount> Handle(RegisterCompanyAccountCommand command, CancellationToken cancellationToken = default);
}
