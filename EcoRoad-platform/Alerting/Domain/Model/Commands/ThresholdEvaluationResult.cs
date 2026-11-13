using EcoRoad.Alerting.Domain.Model.Aggregates;

namespace EcoRoad.Alerting.Domain.Model.Commands;

public record ThresholdEvaluationResult(bool DeviationDetected, bool IsCritical, Alert? Alert);
