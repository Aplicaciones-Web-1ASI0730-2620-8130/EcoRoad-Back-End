using EcoRoad.Monitoring.Domain.Model.Aggregates;
using EcoRoad.Monitoring.Domain.Model.Queries;
using EcoRoad.Monitoring.Domain.Model.ValueObjects;

namespace EcoRoad.Monitoring.Domain.Services;

/// <summary>
/// Domain service that summarizes the measurements of a project into its dashboard.
/// </summary>
public static class MonitoringDashboardBuilder
{
    /// <summary>
    /// Builds the dashboard from the latest reading of every section and parameter.
    /// </summary>
    /// <param name="projectId">Identifier of the road project.</param>
    /// <param name="measurements">Recent measurements of the project.</param>
    /// <returns>The project dashboard.</returns>
    public static MonitoringDashboard Build(string projectId, IEnumerable<EnvironmentalMeasurement> measurements)
    {
        var current = measurements
            .Where(measurement => measurement.ProjectId == projectId)
            .GroupBy(measurement => (measurement.SectionId, measurement.ParameterType))
            .Select(group => group.OrderByDescending(measurement => measurement.RecordedAt).First())
            .Select(ToSnapshot)
            .ToList();

        var sections = current
            .GroupBy(reading => reading.SectionId)
            .OrderBy(group => group.Key)
            .Select(group => new SectionIndicator(
                group.Key,
                group.Max(reading => reading.Status),
                group.OrderBy(reading => reading.ParameterType).ToList()))
            .ToList();

        var parameters = Enum.GetValues<ParameterType>()
            .Select(parameter => new ParameterIndicator(
                parameter,
                parameter.Metric(),
                parameter.Unit(),
                current
                    .Where(reading => reading.ParameterType == parameter)
                    .OrderByDescending(reading => reading.Status)
                    .ThenByDescending(reading => reading.RecordedAt)
                    .FirstOrDefault()))
            .ToList();

        return new MonitoringDashboard(
            projectId,
            current.Count == 0 ? null : current.Max(reading => reading.Status),
            current.Count == 0 ? null : current.Max(reading => reading.RecordedAt),
            parameters,
            sections);
    }

    /// <summary>
    /// Converts a measurement into its dashboard snapshot.
    /// </summary>
    /// <param name="measurement">The measurement.</param>
    /// <returns>The snapshot.</returns>
    public static ReadingSnapshot ToSnapshot(EnvironmentalMeasurement measurement)
    {
        return new ReadingSnapshot(
            measurement.Id,
            measurement.SectionId,
            measurement.ParameterType,
            measurement.Value,
            measurement.Unit,
            measurement.Status,
            measurement.Source,
            measurement.RecordedAt);
    }
}
