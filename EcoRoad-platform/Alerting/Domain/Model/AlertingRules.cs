namespace EcoRoad.Alerting.Domain.Model;

internal static class AlertingRules
{
    public static string Require(string? value, string field, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new AlertingRuleException($"{field} is required.");
        }

        var trimmed = value.Trim();
        if (trimmed.Length > maxLength)
        {
            throw new AlertingRuleException($"{field} exceeds {maxLength} characters.");
        }

        return trimmed;
    }

    public static string RequireId(string? value, string field)
    {
        var trimmed = Require(value, field, 36);
        if (trimmed.Length != 36)
        {
            throw new AlertingRuleException($"{field} must be 36 characters.");
        }

        return trimmed;
    }

    public static double RequireFinite(double value, string field)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            throw new AlertingRuleException($"{field} must be a finite number.");
        }

        return value;
    }

    public static double RequireNonNegative(double value, string field)
    {
        var finite = RequireFinite(value, field);
        if (finite < 0)
        {
            throw new AlertingRuleException($"{field} cannot be negative.");
        }

        return finite;
    }
}
