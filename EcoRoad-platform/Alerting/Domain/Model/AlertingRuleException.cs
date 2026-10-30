namespace EcoRoad.Alerting.Domain.Model;

public class AlertingRuleException : Exception
{
    public AlertingRuleException(string message, bool notFound = false)
        : base(message)
    {
        NotFound = notFound;
    }

    public bool NotFound { get; }
}
