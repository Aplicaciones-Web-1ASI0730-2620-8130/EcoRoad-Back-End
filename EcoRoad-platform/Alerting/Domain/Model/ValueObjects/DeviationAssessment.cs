namespace EcoRoad.Alerting.Domain.Model.ValueObjects;

public class DeviationAssessment
{
    private DeviationAssessment(
        bool isDeviation,
        bool isCritical,
        double measuredValue,
        double limitValue)
    {
        IsDeviation = isDeviation;
        IsCritical = isCritical;
        MeasuredValue = measuredValue;
        LimitValue = limitValue;
    }

    public bool IsDeviation { get; }

    public bool IsCritical { get; }

    public double MeasuredValue { get; }

    public double LimitValue { get; }

    public static DeviationAssessment WithinLimit(double measuredValue, double limitValue)
    {
        return new DeviationAssessment(false, false, measuredValue, limitValue);
    }

    public static DeviationAssessment Detected(double measuredValue, double limitValue, bool isCritical)
    {
        return new DeviationAssessment(true, isCritical, measuredValue, limitValue);
    }
}
