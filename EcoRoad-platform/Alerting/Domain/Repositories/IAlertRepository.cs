using EcoRoad.Alerting.Domain.Model.Aggregates;

namespace EcoRoad.Alerting.Domain.Repositories;

public interface IAlertRepository
{
    Task<Alert?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Alert>> GetActiveAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Alert alert, CancellationToken cancellationToken = default);

    Task UpdateAsync(Alert alert, CancellationToken cancellationToken = default);
}
