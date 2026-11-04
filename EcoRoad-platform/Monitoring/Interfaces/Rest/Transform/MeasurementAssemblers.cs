using EcoRoad.Monitoring.Domain.Model.Aggregates;
using EcoRoad.Monitoring.Domain.Model.Commands;
using EcoRoad.Monitoring.Domain.Model.Queries;
using EcoRoad.Monitoring.Interfaces.Rest.Resources;

namespace EcoRoad.Monitoring.Interfaces.Rest.Transform;

/// <summary>
/// Converts measurements and dashboards between their domain and REST representations.
/// </summary>
public static class MeasurementAssembler
{
    /// <summary>
    /// Converts a measurement into its resource.
    /// </summary>
    /// <param name="measurement">The measurement to convert.</param>
    /// <returns>The measurement resource.</returns>
    public static MeasurementResource ToResourceFromEntity(EnvironmentalMeasurement measurement)
    {
        return new MeasurementResource(
            measurement.Id,
            measurement.ProjectId,
            measurement.SectionId,
            measurement.MonitoringPointId,
            measurement.SensorId,
            measurement.RecordedBy,
            measurement.ParameterType,
            measurement.Value,
            measurement.Unit,
            measurement.Source,
            measurement.Notes,
            measurement.RecordedAt,
            measurement.Status,
            measurement.ThresholdProfileId);
    }

    /// <summary>
    /// Builds the telemetry command.
    /// </summary>
    /// <param name="resource">The request body.</param>
    /// <returns>The command.</returns>
    public static CaptureTelemetryReadingCommand ToTelemetryCommand(CaptureTelemetryReadingResource resource)
    {
        return new CaptureTelemetryReadingCommand(
            resource.ProjectId,
            resource.SectionId,
            resource.MonitoringPointId,
            resource.SensorId,
            resource.ParameterType,
            resource.Value,
            resource.RecordedAt);
    }

    /// <summary>
    /// Builds the manual measurement command.
    /// </summary>
    /// <param name="resource">The request body.</param>
    /// <returns>The command.</returns>
    public static RecordManualMeasurementCommand ToManualCommand(RecordManualMeasurementResource resource)
    {
        return new RecordManualMeasurementCommand(
            resource.ProjectId,
            resource.SectionId,
            resource.OperatorId,
            resource.ParameterType,
            resource.Value,
            resource.RecordedAt,
            resource.Notes);
    }

    /// <summary>
    /// Converts a project dashboard into its resource.
    /// </summary>
    /// <param name="dashboard">The dashboard read model.</param>
    /// <returns>The dashboard resource.</returns>
    public static MonitoringDashboardResource ToDashboardResource(MonitoringDashboard dashboard)
    {
        return new MonitoringDashboardResource(
            dashboard.ProjectId,
            dashboard.Status,
            dashboard.LatestAt,
            dashboard.Parameters
                .Select(indicator => new ParameterIndicatorResource(
                    indicator.ParameterType,
                    indicator.Metric,
                    indicator.Unit,
                    indicator.WorstReading == null ? null : ToReadingResource(indicator.WorstReading)))
                .ToList(),
            dashboard.Sections
                .Select(section => new SectionIndicatorResource(
                    section.SectionId,
                    section.Status,
                    section.Readings.Select(ToReadingResource).ToList()))
                .ToList());
    }

    /// <summary>
    /// Converts a reading snapshot into its resource.
    /// </summary>
    /// <param name="reading">The reading snapshot.</param>
    /// <returns>The reading resource.</returns>
    private static ReadingResource ToReadingResource(ReadingSnapshot reading)
    {
        return new ReadingResource(
            reading.MeasurementId,
            reading.SectionId,
            reading.ParameterType,
            reading.Value,
            reading.Unit,
            reading.Status,
            reading.Source,
            reading.RecordedAt);
    }
}
