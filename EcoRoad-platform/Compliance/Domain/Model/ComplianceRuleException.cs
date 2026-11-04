namespace EcoRoad.Compliance.Domain.Model;

public class ComplianceRuleException : Exception
{
    public ComplianceRuleException(string message, bool notFound = false)
        : base(message)
    {
        NotFound = notFound;
    }

    public bool NotFound { get; }
}
