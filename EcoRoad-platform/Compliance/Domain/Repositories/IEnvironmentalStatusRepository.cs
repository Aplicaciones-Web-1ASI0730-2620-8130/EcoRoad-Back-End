using EcoRoad.Compliance.Domain.Model.Aggregates;
using EcoRoad.Compliance.Domain.Model.ValueObjects;

namespace EcoRoad.Compliance.Domain.Repositories;

public interface IEnvironmentalStatusRepository
{
    Task<EnvironmentalStatus?> GetBySubjectAsync(
        ComplianceScope scope,
        string subjectId,
        CancellationToken cancellationToken = default);

    Task AddAsync(EnvironmentalStatus status, CancellationToken cancellationToken = default);

    Task UpdateAsync(EnvironmentalStatus status, CancellationToken cancellationToken = default);
}
