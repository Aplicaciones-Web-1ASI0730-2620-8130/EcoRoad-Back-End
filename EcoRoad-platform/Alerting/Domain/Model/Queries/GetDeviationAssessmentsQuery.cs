namespace EcoRoad.Alerting.Domain.Model.Queries;

/// <summary>
///     Reads deviation assessments, optionally limited to one measurement.
/// </summary>
public record GetDeviationAssessmentsQuery(string? MeasurementId);
