using EcoRoad.Alerting.Domain.Model.Aggregates;
using EcoRoad.Alerting.Domain.Model.Commands;

namespace EcoRoad.Alerting.Application.CommandServices;

public interface IAlertingCommandService
{
    Task<ThresholdEvaluationResult> Handle(
        EvaluateThresholdDeviationCommand command,
        CancellationToken cancellationToken = default);

    Task<Alert> Handle(AcknowledgeAlertCommand command, CancellationToken cancellationToken = default);
}
