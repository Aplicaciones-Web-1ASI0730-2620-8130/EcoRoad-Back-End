using EcoRoad.Projects.Domain.Model.Aggregates;

namespace EcoRoad.Projects.Domain.Repositories;

public interface IWorkFrontRepository
{
    Task<IReadOnlyList<WorkFront>> GetBySectionIdAsync(
        string sectionId,
        CancellationToken cancellationToken = default);

    Task<WorkFront?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task AddAsync(WorkFront workFront, CancellationToken cancellationToken = default);

    Task UpdateAsync(WorkFront workFront, CancellationToken cancellationToken = default);

    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}
