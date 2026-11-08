namespace EcoRoad.Alerting.Domain.Model.Queries;

/// <summary>
///     Reads one deviation assessment by its id.
/// </summary>
public record GetDeviationAssessmentByIdQuery(string AssessmentId);
