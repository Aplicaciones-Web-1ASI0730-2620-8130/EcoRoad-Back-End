using EcoRoad.Monitoring.Domain.Model.ValueObjects;

namespace EcoRoad.Monitoring.Domain.Model.Queries;

/// <summary>
/// Read model of a single measurement shown in dashboards.
/// </summary>
/// <param name="MeasurementId">Identifier of the measurement.</param>
/// <param name="SectionId">Identifier of the road section.</param>
/// <param name="ParameterType">Measured parameter.</param>
/// <param name="Value">Measured value.</param>
/// <param name="Unit">Unit of the value.</param>
/// <param name="Status">Status against the threshold profile.</param>
/// <param name="Source">Origin of the measurement.</param>
/// <param name="RecordedAt">UTC date of the measurement.</param>
public record ReadingSnapshot(
    string MeasurementId,
    string SectionId,
    ParameterType ParameterType,
    double Value,
    string Unit,
    MonitoringStatus Status,
    MeasurementSource Source,
    DateTime RecordedAt);
