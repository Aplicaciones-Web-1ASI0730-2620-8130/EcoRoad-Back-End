using EcoRoad.Monitoring.Domain.Model.ValueObjects;
using EcoRoad.Shared.Domain.Model.Entities;
using EcoRoad.Shared.Domain.Model.Exceptions;

namespace EcoRoad.Monitoring.Domain.Model.Aggregates;

/// <summary>
/// Environmental measurement captured by a sensor or recorded manually on site.
/// </summary>
/// <remarks>
/// Measurements are immutable evidence for compliance audits: they expose no update method and
/// keep the status they had against the threshold profile in force when they were recorded.
/// </remarks>
public class EnvironmentalMeasurement : IAuditableEntity
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    private EnvironmentalMeasurement()
    {
    }

    /// <summary>
    /// Initializes a measurement and classifies it against its threshold profile.
    /// </summary>
    /// <param name="projectId">Identifier of the road project.</param>
    /// <param name="sectionId">Identifier of the road section.</param>
    /// <param name="value">The measured value.</param>
    /// <param name="source">Origin of the measurement.</param>
    /// <param name="recordedAt">UTC date of the measurement.</param>
    /// <param name="profile">Threshold profile of the parameter.</param>
    /// <param name="now">Current UTC date and time.</param>
    private EnvironmentalMeasurement(
        string projectId,
        string sectionId,
        double value,
        MeasurementSource source,
        DateTime recordedAt,
        ThresholdProfile profile,
        DateTime now)
    {
        Id = Guid.NewGuid().ToString();
        ProjectId = MonitoringRules.Require(projectId, "Project id", 36, "INVALID_MEASUREMENT");
        SectionId = MonitoringRules.Require(sectionId, "Section id", 36, "INVALID_MEASUREMENT");
        ParameterType = profile.ParameterType;
        Unit = profile.Unit;
        Value = MonitoringRules.RequirePlausibleValue(profile.ParameterType, value);
        Source = source;
        RecordedAt = MonitoringRules.RequirePastDate(recordedAt, now);
        EvaluateReadingAgainstThreshold(profile);
    }

    /// <summary>Gets the unique identifier of the measurement.</summary>
    public string Id { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the road project.</summary>
    public string ProjectId { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the road section.</summary>
    public string SectionId { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the monitoring point, for automatic readings.</summary>
    public string? MonitoringPointId { get; private set; }

    /// <summary>Gets the identifier of the sensor, for automatic readings.</summary>
    public string? SensorId { get; private set; }

    /// <summary>Gets the identifier of the operator, for manual records.</summary>
    public string? RecordedBy { get; private set; }

    /// <summary>Gets the measured environmental parameter.</summary>
    public ParameterType ParameterType { get; private set; }

    /// <summary>Gets the measured value.</summary>
    public double Value { get; private set; }

    /// <summary>Gets the unit of the value.</summary>
    public string Unit { get; private set; } = string.Empty;

    /// <summary>Gets the origin of the measurement.</summary>
    public MeasurementSource Source { get; private set; }

    /// <summary>Gets the field notes of a manual record.</summary>
    public string? Notes { get; private set; }

    /// <summary>Gets the UTC date of the measurement.</summary>
    public DateTime RecordedAt { get; private set; }

    /// <summary>Gets the status of the value against the profile in force when it was recorded.</summary>
    public MonitoringStatus Status { get; private set; }

    /// <summary>Gets the identifier of the threshold profile used for the classification.</summary>
    public string ThresholdProfileId { get; private set; } = string.Empty;

    /// <inheritdoc />
    public DateTime? CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Captures a reading sent automatically by an IoT sensor.
    /// </summary>
    /// <param name="projectId">Identifier of the road project.</param>
    /// <param name="sectionId">Identifier of the road section.</param>
    /// <param name="monitoringPointId">Identifier of the monitoring point.</param>
    /// <param name="sensorId">Identifier of the sensor.</param>
    /// <param name="value">The measured value.</param>
    /// <param name="recordedAt">UTC date of the reading.</param>
    /// <param name="profile">Threshold profile of the parameter.</param>
    /// <param name="now">Current UTC date and time.</param>
    /// <returns>The classified measurement.</returns>
    public static EnvironmentalMeasurement CaptureEnvironmentalReading(
        string projectId,
        string sectionId,
        string monitoringPointId,
        string sensorId,
        double value,
        DateTime recordedAt,
        ThresholdProfile profile,
        DateTime now)
    {
        return new EnvironmentalMeasurement(projectId, sectionId, value, MeasurementSource.AutomaticSensor, recordedAt, profile, now)
        {
            MonitoringPointId = MonitoringRules.Require(monitoringPointId, "Monitoring point id", 36, "INVALID_MEASUREMENT"),
            SensorId = MonitoringRules.Require(sensorId, "Sensor id", 36, "INVALID_MEASUREMENT")
        };
    }

    /// <summary>
    /// Records a measurement taken manually by an operator on site.
    /// </summary>
    /// <param name="projectId">Identifier of the road project.</param>
    /// <param name="sectionId">Identifier of the road section.</param>
    /// <param name="operatorId">Identifier of the operator.</param>
    /// <param name="value">The measured value.</param>
    /// <param name="recordedAt">UTC date of the measurement.</param>
    /// <param name="notes">Optional field notes.</param>
    /// <param name="profile">Threshold profile of the parameter.</param>
    /// <param name="now">Current UTC date and time.</param>
    /// <returns>The classified measurement.</returns>
    public static EnvironmentalMeasurement RecordManualMeasurement(
        string projectId,
        string sectionId,
        string operatorId,
        double value,
        DateTime recordedAt,
        string? notes,
        ThresholdProfile profile,
        DateTime now)
    {
        return new EnvironmentalMeasurement(projectId, sectionId, value, MeasurementSource.ManualEntry, recordedAt, profile, now)
        {
            RecordedBy = MonitoringRules.Require(operatorId, "Operator id", 36, "INVALID_MEASUREMENT"),
            Notes = MonitoringRules.Optional(notes, "Notes", 500, "INVALID_MEASUREMENT")
        };
    }

    /// <summary>
    /// Classifies the measurement against a threshold profile of the same parameter.
    /// </summary>
    /// <param name="profile">The threshold profile.</param>
    /// <returns><c>true</c> when the value is outside the optimal band.</returns>
    public bool EvaluateReadingAgainstThreshold(ThresholdProfile profile)
    {
        if (profile.ParameterType != ParameterType)
        {
            throw new MonitoringRuleException(
                "PROFILE_MISMATCH",
                "The threshold profile belongs to another parameter.",
                DomainErrorKind.Conflict);
        }

        Status = profile.Classify(Value);
        ThresholdProfileId = profile.Id;
        return Status != MonitoringStatus.Optimal;
    }
}
