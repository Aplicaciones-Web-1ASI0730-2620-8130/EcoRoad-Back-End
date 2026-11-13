using EcoRoad.Alerting.Domain.Model.ValueObjects;

namespace EcoRoad.Alerting.Domain.Model.Commands;

public record RegisterAlertRuleCommand(
    string Parameter,
    string Unit,
    TimeBand TimeBand,
    double Limit,
    double CriticalExcess);
