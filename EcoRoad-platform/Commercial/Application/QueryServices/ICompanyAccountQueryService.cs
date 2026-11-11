using EcoRoad.Commercial.Domain.Model.Aggregates;
using EcoRoad.Commercial.Domain.Model.Queries;

namespace EcoRoad.Commercial.Application.QueryServices;

public interface ICompanyAccountQueryService
{
    Task<CompanyAccount?> Handle(GetCompanyAccountQuery query, CancellationToken cancellationToken = default);
}
