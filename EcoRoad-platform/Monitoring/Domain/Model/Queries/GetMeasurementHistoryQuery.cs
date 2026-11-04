using EcoRoad.Monitoring.Domain.Model.ValueObjects;

namespace EcoRoad.Monitoring.Domain.Model.Queries;

/// <summary>
/// Lists the measurements of a parameter in a road project within a date range.
/// </summary>
/// <param name="ProjectId">Identifier of the road project.</param>
/// <param name="SectionId">Optional road section filter.</param>
/// <param name="ParameterType">The environmental parameter.</param>
/// <param name="From">Optional first day of the range, inclusive.</param>
/// <param name="To">Optional last day of the range, inclusive.</param>
public record GetMeasurementHistoryQuery(
    string ProjectId,
    string? SectionId,
    ParameterType ParameterType,
    DateOnly? From,
    DateOnly? To);
