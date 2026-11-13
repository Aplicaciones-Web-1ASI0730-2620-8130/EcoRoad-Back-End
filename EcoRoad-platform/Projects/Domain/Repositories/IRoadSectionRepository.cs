using EcoRoad.Projects.Domain.Model.Aggregates;

namespace EcoRoad.Projects.Domain.Repositories;

public interface IRoadSectionRepository
{
    Task<RoadSection?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<RoadSection?> GetByIdWithWorkFrontsAsync(string id, CancellationToken cancellationToken = default);

    Task AddAsync(RoadSection section, CancellationToken cancellationToken = default);

    Task UpdateAsync(RoadSection section, CancellationToken cancellationToken = default);

    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}
