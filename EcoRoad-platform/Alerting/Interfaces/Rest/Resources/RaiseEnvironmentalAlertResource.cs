using EcoRoad.Alerting.Domain.Model.ValueObjects;

namespace EcoRoad.Alerting.Interfaces.Rest.Resources;

/// <summary>
///     Request body used to raise an alert from a stored deviation assessment.
/// </summary>
public record RaiseEnvironmentalAlertResource(
    string AssessmentId,
    string MonitoringPointId,
    AlertSeverity Severity,
    string Message);
