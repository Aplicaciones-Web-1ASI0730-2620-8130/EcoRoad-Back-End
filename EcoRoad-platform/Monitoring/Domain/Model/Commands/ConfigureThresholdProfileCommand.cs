using EcoRoad.Monitoring.Domain.Model.ValueObjects;

namespace EcoRoad.Monitoring.Domain.Model.Commands;

/// <summary>
/// Configures the threshold profile of an environmental parameter.
/// </summary>
/// <param name="Name">Descriptive name, for example the standard it comes from.</param>
/// <param name="ParameterType">The environmental parameter.</param>
/// <param name="Limits">The limits used to classify the measurements.</param>
public record ConfigureThresholdProfileCommand(
    string Name,
    ParameterType ParameterType,
    ThresholdLimits Limits);
