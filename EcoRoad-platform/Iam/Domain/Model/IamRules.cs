using System.Text.RegularExpressions;

namespace EcoRoad.Iam.Domain.Model;

/// <summary>
/// Reusable guard clauses that protect the invariants of the IAM aggregates.
/// </summary>
internal static partial class IamRules
{
    /// <summary>Minimum length accepted for a password.</summary>
    public const int MinimumPasswordLength = 8;

    /// <summary>
    /// Ensures a text value is present and within the allowed length.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="field">Name of the field, used in the error message.</param>
    /// <param name="maxLength">Maximum number of characters allowed.</param>
    /// <returns>The trimmed value.</returns>
    public static string Require(string? value, string field, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new IamRuleException("FIELD_REQUIRED", $"{field} is required.");
        }

        var trimmed = value.Trim();
        if (trimmed.Length > maxLength)
        {
            throw new IamRuleException("FIELD_TOO_LONG", $"{field} exceeds {maxLength} characters.");
        }

        return trimmed;
    }

    /// <summary>
    /// Ensures an email address is well formed and returns it in its canonical lower case form.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns>The normalized email address.</returns>
    public static string RequireEmail(string? email)
    {
        var normalized = Require(email, "Email", 120).ToLowerInvariant();
        if (!EmailPattern().IsMatch(normalized))
        {
            throw new IamRuleException("INVALID_EMAIL", "Email must be a valid address.");
        }

        return normalized;
    }

    /// <summary>
    /// Ensures a password satisfies the minimum strength policy of the platform.
    /// </summary>
    /// <param name="password">The plain text password to validate.</param>
    /// <returns>The same password when it is valid.</returns>
    public static string RequireStrongPassword(string? password)
    {
        if (string.IsNullOrEmpty(password) || password.Length < MinimumPasswordLength)
        {
            throw new IamRuleException(
                "WEAK_PASSWORD",
                $"Password must contain at least {MinimumPasswordLength} characters.");
        }

        var hasUpper = password.Any(char.IsUpper);
        var hasLower = password.Any(char.IsLower);
        var hasDigit = password.Any(char.IsDigit);
        if (!hasUpper || !hasLower || !hasDigit)
        {
            throw new IamRuleException(
                "WEAK_PASSWORD",
                "Password must combine upper case letters, lower case letters and digits.");
        }

        return password;
    }

    /// <summary>
    /// Ensures an enum value is one of the defined members.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="code">Error code raised when the value is not defined.</param>
    /// <param name="field">Name of the field, used in the error message.</param>
    /// <returns>The same value when it is defined.</returns>
    public static TEnum RequireDefined<TEnum>(TEnum value, string code, string field)
        where TEnum : struct, Enum
    {
        if (!Enum.IsDefined(value))
        {
            throw new IamRuleException(code, $"{field} is not valid.");
        }

        return value;
    }

    [GeneratedRegex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$")]
    private static partial Regex EmailPattern();
}
