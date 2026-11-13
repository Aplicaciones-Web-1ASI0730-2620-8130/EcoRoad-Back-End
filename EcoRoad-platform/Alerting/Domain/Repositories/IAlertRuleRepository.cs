using EcoRoad.Alerting.Domain.Model.Aggregates;
using EcoRoad.Alerting.Domain.Model.ValueObjects;

namespace EcoRoad.Alerting.Domain.Repositories;

public interface IAlertRuleRepository
{
    Task<AlertRule?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<AlertRule?> GetByParameterAndTimeBandAsync(
        string parameter,
        TimeBand timeBand,
        CancellationToken cancellationToken = default);

    Task AddAsync(AlertRule rule, CancellationToken cancellationToken = default);

    Task UpdateAsync(AlertRule rule, CancellationToken cancellationToken = default);
}
