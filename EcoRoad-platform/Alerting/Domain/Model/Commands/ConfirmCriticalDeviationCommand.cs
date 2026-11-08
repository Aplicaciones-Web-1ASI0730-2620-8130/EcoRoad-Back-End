namespace EcoRoad.Alerting.Domain.Model.Commands;

/// <summary>
///     Confirms that an existing deviation assessment is critical.
/// </summary>
public record ConfirmCriticalDeviationCommand(string AssessmentId);
