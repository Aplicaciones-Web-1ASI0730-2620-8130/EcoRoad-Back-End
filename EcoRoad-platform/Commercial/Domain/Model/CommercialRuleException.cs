namespace EcoRoad.Commercial.Domain.Model;

public class CommercialRuleException : Exception
{
    public CommercialRuleException(string message, bool notFound = false)
        : base(message)
    {
        NotFound = notFound;
    }

    public bool NotFound { get; }
}
