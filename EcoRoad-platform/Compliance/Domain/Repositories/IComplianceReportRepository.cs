using EcoRoad.Compliance.Domain.Model.Aggregates;

namespace EcoRoad.Compliance.Domain.Repositories;

public interface IComplianceReportRepository
{
    Task<ComplianceReport?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task AddAsync(ComplianceReport report, CancellationToken cancellationToken = default);

    Task UpdateAsync(ComplianceReport report, CancellationToken cancellationToken = default);
}
