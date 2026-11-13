namespace EcoRoad.Compliance.Domain.Model;

internal static class ComplianceRules
{
    public static string Require(string? value, string field, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ComplianceRuleException($"{field} is required.");
        }

        var trimmed = value.Trim();
        if (trimmed.Length > maxLength)
        {
            throw new ComplianceRuleException($"{field} exceeds {maxLength} characters.");
        }

        return trimmed;
    }

    public static string RequireId(string? value, string field)
    {
        var trimmed = Require(value, field, 36);
        if (trimmed.Length != 36)
        {
            throw new ComplianceRuleException($"{field} must be 36 characters.");
        }

        return trimmed;
    }

    public static string RequireSha256(string? value)
    {
        var trimmed = Require(value, "Content hash", 64);
        if (trimmed.Length != 64 || trimmed.Any(character => !Uri.IsHexDigit(character)))
        {
            throw new ComplianceRuleException("Content hash must be a SHA-256 hex string.");
        }

        return trimmed.ToLowerInvariant();
    }

    public static IReadOnlyList<string> RequireIds(IEnumerable<string>? values, string field)
    {
        if (values == null)
        {
            throw new ComplianceRuleException($"{field} is required.");
        }

        return values.Select(value => RequireId(value, field)).ToList();
    }
}
