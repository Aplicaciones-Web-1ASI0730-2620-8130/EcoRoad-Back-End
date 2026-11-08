using EcoRoad.Alerting.Domain.Model.Aggregates;

namespace EcoRoad.Alerting.Domain.Model.Commands;

/// <summary>
///     Outcome of a threshold evaluation, including the alert when a deviation is raised.
/// </summary>
public record ThresholdEvaluationResult(bool DeviationDetected, bool IsCritical, Alert? Alert);
