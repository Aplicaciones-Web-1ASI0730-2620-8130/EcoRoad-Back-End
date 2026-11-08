using EcoRoad.Alerting.Domain.Model.Aggregates;
using EcoRoad.Alerting.Domain.Model.Commands;

namespace EcoRoad.Alerting.Application.CommandServices;

/// <summary>
///     Command service for threshold evaluation and environmental alerts.
/// </summary>
public interface IAlertingCommandService
{
    /// <summary>
    ///     Evaluates a measurement and raises an alert when the threshold is exceeded.
    /// </summary>
    Task<ThresholdEvaluationResult> Handle(
        EvaluateThresholdDeviationCommand command,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Confirms that a stored deviation assessment is critical.
    /// </summary>
    Task<DeviationAssessment> Handle(
        ConfirmCriticalDeviationCommand command,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Raises an environmental alert originated by a stored deviation assessment.
    /// </summary>
    Task<Alert> Handle(
        RaiseEnvironmentalAlertCommand command,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Acknowledges an alert on behalf of a user.
    /// </summary>
    Task<Alert> Handle(AcknowledgeAlertCommand command, CancellationToken cancellationToken = default);
}
