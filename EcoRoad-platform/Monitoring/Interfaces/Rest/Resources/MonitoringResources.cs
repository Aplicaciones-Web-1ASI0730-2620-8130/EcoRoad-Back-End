using EcoRoad.Monitoring.Domain.Model.ValueObjects;

namespace EcoRoad.Monitoring.Interfaces.Rest.Resources;

/// <summary>
/// Limits of a threshold profile.
/// </summary>
/// <param name="Kind"><c>Maximum</c> uses the warning and critical thresholds; <c>Range</c> uses the four band limits.</param>
/// <param name="WarningThreshold">Value from which a maximum profile becomes <c>Observation</c>.</param>
/// <param name="CriticalThreshold">Value from which a maximum profile becomes <c>Critical</c>.</param>
/// <param name="Minimum">Lowest acceptable value of a range profile.</param>
/// <param name="OptimalMinimum">Lowest optimal value of a range profile.</param>
/// <param name="OptimalMaximum">Highest optimal value of a range profile.</param>
/// <param name="Maximum">Highest acceptable value of a range profile.</param>
public record ThresholdLimitsResource(
    ThresholdKind Kind,
    double? WarningThreshold,
    double? CriticalThreshold,
    double? Minimum,
    double? OptimalMinimum,
    double? OptimalMaximum,
    double? Maximum);

/// <summary>
/// Request body to configure the threshold profile of a parameter.
/// </summary>
/// <param name="Name">Descriptive name, for example the standard it comes from.</param>
/// <param name="ParameterType">The environmental parameter.</param>
/// <param name="Limits">The limits of the profile.</param>
public record ConfigureThresholdProfileResource(string Name, ParameterType ParameterType, ThresholdLimitsResource Limits);

/// <summary>
/// Threshold profile returned by the API.
/// </summary>
/// <param name="Id">Identifier of the profile.</param>
/// <param name="Name">Descriptive name.</param>
/// <param name="ParameterType">The environmental parameter.</param>
/// <param name="Metric">Name of the metric.</param>
/// <param name="Unit">Unit of the limits.</param>
/// <param name="Limits">The limits of the profile.</param>
/// <param name="UpdatedAt">UTC date of the last change.</param>
public record ThresholdProfileResource(
    string Id,
    string Name,
    ParameterType ParameterType,
    string Metric,
    string Unit,
    ThresholdLimitsResource Limits,
    DateTime? UpdatedAt);

/// <summary>
/// Request body sent by an IoT sensor, or its gateway, to report a reading.
/// </summary>
/// <param name="ProjectId">Identifier of the road project.</param>
/// <param name="SectionId">Identifier of the road section.</param>
/// <param name="MonitoringPointId">Identifier of the monitoring point.</param>
/// <param name="SensorId">Identifier of the sensor.</param>
/// <param name="ParameterType">The measured parameter.</param>
/// <param name="Value">The measured value.</param>
/// <param name="RecordedAt">UTC date of the reading; the reception time is used when omitted.</param>
public record CaptureTelemetryReadingResource(
    string ProjectId,
    string SectionId,
    string MonitoringPointId,
    string SensorId,
    ParameterType ParameterType,
    double Value,
    DateTime? RecordedAt);

/// <summary>
/// Request body to record a manual field measurement.
/// </summary>
/// <param name="ProjectId">Identifier of the road project.</param>
/// <param name="SectionId">Identifier of the road section.</param>
/// <param name="OperatorId">Identifier of the operator.</param>
/// <param name="ParameterType">The measured parameter.</param>
/// <param name="Value">The measured value.</param>
/// <param name="RecordedAt">UTC date of the measurement; the reception time is used when omitted.</param>
/// <param name="Notes">Optional field notes.</param>
public record RecordManualMeasurementResource(
    string ProjectId,
    string SectionId,
    string OperatorId,
    ParameterType ParameterType,
    double Value,
    DateTime? RecordedAt,
    string? Notes);

/// <summary>
/// Measurement returned by the API.
/// </summary>
/// <param name="Id">Identifier of the measurement.</param>
/// <param name="ProjectId">Identifier of the road project.</param>
/// <param name="SectionId">Identifier of the road section.</param>
/// <param name="MonitoringPointId">Monitoring point of an automatic reading.</param>
/// <param name="SensorId">Sensor of an automatic reading.</param>
/// <param name="RecordedBy">Operator of a manual record.</param>
/// <param name="ParameterType">The measured parameter.</param>
/// <param name="Value">The measured value.</param>
/// <param name="Unit">Unit of the value.</param>
/// <param name="Source">Origin of the measurement.</param>
/// <param name="Notes">Field notes.</param>
/// <param name="RecordedAt">UTC date of the measurement.</param>
/// <param name="Status">Status against the threshold profile.</param>
/// <param name="ThresholdProfileId">Profile used for the classification.</param>
public record MeasurementResource(
    string Id,
    string ProjectId,
    string SectionId,
    string? MonitoringPointId,
    string? SensorId,
    string? RecordedBy,
    ParameterType ParameterType,
    double Value,
    string Unit,
    MeasurementSource Source,
    string? Notes,
    DateTime RecordedAt,
    MonitoringStatus Status,
    string ThresholdProfileId);

/// <summary>
/// Current reading shown in the dashboard.
/// </summary>
/// <param name="MeasurementId">Identifier of the measurement.</param>
/// <param name="SectionId">Identifier of the road section.</param>
/// <param name="ParameterType">The measured parameter.</param>
/// <param name="Value">The measured value.</param>
/// <param name="Unit">Unit of the value.</param>
/// <param name="Status">Status against the threshold profile.</param>
/// <param name="Source">Origin of the measurement.</param>
/// <param name="RecordedAt">UTC date of the measurement.</param>
public record ReadingResource(
    string MeasurementId,
    string SectionId,
    ParameterType ParameterType,
    double Value,
    string Unit,
    MonitoringStatus Status,
    MeasurementSource Source,
    DateTime RecordedAt);

/// <summary>
/// Dashboard indicator of a parameter.
/// </summary>
/// <param name="ParameterType">The environmental parameter.</param>
/// <param name="Metric">Name of the metric.</param>
/// <param name="Unit">Unit of the parameter.</param>
/// <param name="WorstReading">Worst current reading across the sections.</param>
public record ParameterIndicatorResource(ParameterType ParameterType, string Metric, string Unit, ReadingResource? WorstReading);

/// <summary>
/// Dashboard indicator of a road section.
/// </summary>
/// <param name="SectionId">Identifier of the road section.</param>
/// <param name="Status">Worst status of the current readings.</param>
/// <param name="Readings">Latest reading of every parameter measured in the section.</param>
public record SectionIndicatorResource(string SectionId, MonitoringStatus Status, IReadOnlyList<ReadingResource> Readings);

/// <summary>
/// Environmental dashboard of a road project.
/// </summary>
/// <param name="ProjectId">Identifier of the road project.</param>
/// <param name="Status">Worst current status of the project.</param>
/// <param name="LatestAt">UTC date of the most recent reading.</param>
/// <param name="Parameters">Indicator of every parameter.</param>
/// <param name="Sections">Indicator of every section with readings.</param>
public record MonitoringDashboardResource(
    string ProjectId,
    MonitoringStatus? Status,
    DateTime? LatestAt,
    IReadOnlyList<ParameterIndicatorResource> Parameters,
    IReadOnlyList<SectionIndicatorResource> Sections);
