using EcoRoad.Alerting.Domain.Model.ValueObjects;

namespace EcoRoad.Alerting.Domain.Model.Commands;

public record EvaluateThresholdDeviationCommand(
    string WorkFrontId,
    string Parameter,
    TimeBand TimeBand,
    double MeasuredValue);
