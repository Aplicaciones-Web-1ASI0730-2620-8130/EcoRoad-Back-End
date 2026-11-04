using EcoRoad.Monitoring.Domain.Model.ValueObjects;

namespace EcoRoad.Monitoring.Domain.Model.Commands;

/// <summary>
/// Records a measurement taken manually by an operator on site.
/// </summary>
/// <param name="ProjectId">Identifier of the road project.</param>
/// <param name="SectionId">Identifier of the road section.</param>
/// <param name="OperatorId">Identifier of the operator.</param>
/// <param name="ParameterType">The measured parameter.</param>
/// <param name="Value">The measured value.</param>
/// <param name="RecordedAt">UTC date of the measurement; the reception time is used when omitted.</param>
/// <param name="Notes">Optional field notes.</param>
public record RecordManualMeasurementCommand(
    string ProjectId,
    string SectionId,
    string OperatorId,
    ParameterType ParameterType,
    double Value,
    DateTime? RecordedAt,
    string? Notes);
