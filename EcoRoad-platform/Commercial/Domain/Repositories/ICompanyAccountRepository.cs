using EcoRoad.Commercial.Domain.Model.Aggregates;

namespace EcoRoad.Commercial.Domain.Repositories;

public interface ICompanyAccountRepository
{
    Task<CompanyAccount?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<CompanyAccount?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    Task AddAsync(CompanyAccount account, CancellationToken cancellationToken = default);

    Task UpdateAsync(CompanyAccount account, CancellationToken cancellationToken = default);
}
