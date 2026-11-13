using EcoRoad.Projects.Domain.Model.Aggregates;

namespace EcoRoad.Projects.Domain.Repositories;

public interface IRoadProjectRepository
{
    Task<IReadOnlyList<RoadProject>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<RoadProject?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<RoadProject?> GetByIdWithSectionsAsync(string id, CancellationToken cancellationToken = default);

    Task AddAsync(RoadProject project, CancellationToken cancellationToken = default);

    Task UpdateAsync(RoadProject project, CancellationToken cancellationToken = default);

    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}
