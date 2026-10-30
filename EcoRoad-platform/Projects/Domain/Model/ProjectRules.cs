namespace EcoRoad.Projects.Domain.Model;

internal static class ProjectRules
{
    public static string Require(string? value, string field, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ProjectRuleException($"{field} is required.");
        }

        var trimmed = value.Trim();
        if (trimmed.Length > maxLength)
        {
            throw new ProjectRuleException($"{field} exceeds {maxLength} characters.");
        }

        return trimmed;
    }

    public static string RegisteredStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            return "registered";
        }

        var trimmed = status.Trim();
        if (trimmed.Length > 30)
        {
            throw new ProjectRuleException("Status exceeds 30 characters.");
        }

        return trimmed;
    }

    public static void RequireKilometerRange(double startKilometer, double endKilometer)
    {
        if (double.IsNaN(startKilometer) || double.IsInfinity(startKilometer)
            || double.IsNaN(endKilometer) || double.IsInfinity(endKilometer))
        {
            throw new ProjectRuleException("Kilometer values must be finite numbers.");
        }

        if (endKilometer <= startKilometer)
        {
            throw new ProjectRuleException("End kilometer must be greater than start kilometer.");
        }
    }
}
