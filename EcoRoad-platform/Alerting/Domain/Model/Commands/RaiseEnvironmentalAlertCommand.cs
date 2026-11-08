using EcoRoad.Alerting.Domain.Model.ValueObjects;

namespace EcoRoad.Alerting.Domain.Model.Commands;

/// <summary>
///     Raises an environmental alert originated by one deviation assessment.
/// </summary>
public record RaiseEnvironmentalAlertCommand(
    string AssessmentId,
    string MonitoringPointId,
    AlertSeverity Severity,
    string Message);
