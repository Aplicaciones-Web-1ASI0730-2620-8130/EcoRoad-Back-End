using EcoRoad.Monitoring.Domain.Model.ValueObjects;

namespace EcoRoad.Monitoring.Domain.Model.Queries;

/// <summary>
/// Dashboard indicator of a parameter: its worst current reading across the project sections.
/// </summary>
/// <param name="ParameterType">The environmental parameter.</param>
/// <param name="Metric">Name of the metric, for example <c>PM10</c>.</param>
/// <param name="Unit">Unit of the parameter.</param>
/// <param name="WorstReading">Worst current reading, or <c>null</c> when there are no readings.</param>
public record ParameterIndicator(ParameterType ParameterType, string Metric, string Unit, ReadingSnapshot? WorstReading);
