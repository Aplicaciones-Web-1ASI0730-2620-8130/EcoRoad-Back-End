namespace EcoRoad.Commercial.Domain.Model;

public class CommercialRuleException : Exception
{
    public CommercialRuleException(string message)
        : base(message)
    {
    }
}
