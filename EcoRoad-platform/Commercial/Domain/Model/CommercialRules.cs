namespace EcoRoad.Commercial.Domain.Model;

internal static class CommercialRules
{
    public static string Require(string? value, string field, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new CommercialRuleException($"{field} is required.");
        }

        var trimmed = value.Trim();
        if (trimmed.Length > maxLength)
        {
            throw new CommercialRuleException($"{field} exceeds {maxLength} characters.");
        }

        return trimmed;
    }

    public static string RequireCompanyCode(string? code)
    {
        var trimmed = Require(code, "Company code", 3);
        if (trimmed.Length != 3)
        {
            throw new CommercialRuleException("Company code must be 3 characters.");
        }

        return trimmed;
    }
}
