using EcoRoad.Commercial.Application.QueryServices;
using EcoRoad.Commercial.Domain.Model.Aggregates;
using EcoRoad.Commercial.Domain.Model.Queries;
using EcoRoad.Commercial.Domain.Repositories;

namespace EcoRoad.Commercial.Application.Internal.QueryServices;

public class CompanyAccountQueryService : ICompanyAccountQueryService
{
    private readonly ICompanyAccountRepository _accounts;

    public CompanyAccountQueryService(ICompanyAccountRepository accounts)
    {
        _accounts = accounts;
    }

    public Task<CompanyAccount?> Handle(
        GetCompanyAccountQuery query,
        CancellationToken cancellationToken = default)
    {
        return _accounts.GetByIdAsync(query.CompanyAccountId, cancellationToken);
    }
}
