using EcoRoad.Monitoring.Domain.Model.ValueObjects;

namespace EcoRoad.Monitoring.Domain.Model.Commands;

/// <summary>
/// Captures a reading sent automatically by an IoT sensor.
/// </summary>
/// <param name="ProjectId">Identifier of the road project.</param>
/// <param name="SectionId">Identifier of the road section.</param>
/// <param name="MonitoringPointId">Identifier of the monitoring point.</param>
/// <param name="SensorId">Identifier of the sensor.</param>
/// <param name="ParameterType">The measured parameter.</param>
/// <param name="Value">The measured value.</param>
/// <param name="RecordedAt">UTC date of the reading; the reception time is used when omitted.</param>
public record CaptureTelemetryReadingCommand(
    string ProjectId,
    string SectionId,
    string MonitoringPointId,
    string SensorId,
    ParameterType ParameterType,
    double Value,
    DateTime? RecordedAt);
