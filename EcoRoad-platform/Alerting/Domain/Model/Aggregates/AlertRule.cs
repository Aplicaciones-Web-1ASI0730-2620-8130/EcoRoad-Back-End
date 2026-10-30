using EcoRoad.Alerting.Domain.Model;
using EcoRoad.Alerting.Domain.Model.ValueObjects;

namespace EcoRoad.Alerting.Domain.Model.Aggregates;

public class AlertRule
{
    public AlertRule()
    {
    }

    public AlertRule(
        string parameter,
        string unit,
        TimeBand timeBand,
        double limit,
        double criticalExcess)
    {
        Id = Guid.NewGuid().ToString();
        Parameter = AlertingRules.Require(parameter, "Parameter", 30);
        Unit = AlertingRules.Require(unit, "Unit", 20);
        TimeBand = timeBand;
        Limit = AlertingRules.RequireFinite(limit, "Limit");
        CriticalExcess = AlertingRules.RequireNonNegative(criticalExcess, "Critical excess");
    }

    public string Id { get; set; } = string.Empty;

    public string Parameter { get; set; } = string.Empty;

    public string Unit { get; set; } = string.Empty;

    public TimeBand TimeBand { get; set; }

    public double Limit { get; set; }

    public double CriticalExcess { get; set; }

    public DeviationAssessment Evaluate(double measuredValue, TimeBand timeBand)
    {
        var measured = AlertingRules.RequireFinite(measuredValue, "Measured value");
        if (timeBand != TimeBand)
        {
            return DeviationAssessment.WithinLimit(measured, Limit);
        }

        if (measured <= Limit)
        {
            return DeviationAssessment.WithinLimit(measured, Limit);
        }

        var isCritical = measured - Limit >= CriticalExcess && CriticalExcess > 0
            || CriticalExcess == 0 && measured > Limit;

        return DeviationAssessment.Detected(measured, Limit, isCritical);
    }
}
