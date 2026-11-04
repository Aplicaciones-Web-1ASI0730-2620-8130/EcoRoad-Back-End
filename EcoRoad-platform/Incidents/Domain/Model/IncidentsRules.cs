namespace EcoRoad.Incidents.Domain.Model;

/// <summary>
/// Reusable guard clauses that protect the invariants of the incident aggregate.
/// </summary>
internal static class IncidentsRules
{
    /// <summary>
    /// Ensures a text value is present and within the allowed length.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="field">Name of the field, used in the error message.</param>
    /// <param name="maxLength">Maximum number of characters allowed.</param>
    /// <param name="code">Error code raised when the value is invalid.</param>
    /// <returns>The trimmed value.</returns>
    public static string Require(string? value, string field, int maxLength, string code)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new IncidentsRuleException(code, $"{field} is required.");
        }

        var trimmed = value.Trim();
        if (trimmed.Length > maxLength)
        {
            throw new IncidentsRuleException(code, $"{field} exceeds {maxLength} characters.");
        }

        return trimmed;
    }

    /// <summary>
    /// Ensures an optional text value is within the allowed length.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="field">Name of the field, used in the error message.</param>
    /// <param name="maxLength">Maximum number of characters allowed.</param>
    /// <param name="code">Error code raised when the value is invalid.</param>
    /// <returns>The trimmed value, or <c>null</c> when it is empty.</returns>
    public static string? Optional(string? value, string field, int maxLength, string code)
    {
        return string.IsNullOrWhiteSpace(value) ? null : Require(value, field, maxLength, code);
    }

    /// <summary>
    /// Ensures an optional file address is an absolute HTTP or HTTPS URL.
    /// </summary>
    /// <param name="value">The address to validate.</param>
    /// <returns>The address, or <c>null</c> when it is empty.</returns>
    public static string? OptionalFileUrl(string? value)
    {
        var url = Optional(value, "File URL", 500, "INVALID_EVIDENCE");
        if (url == null)
        {
            return null;
        }

        var isWebAddress = Uri.TryCreate(url, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp);
        if (!isWebAddress)
        {
            throw new IncidentsRuleException("INVALID_EVIDENCE", "File URL must be an absolute http or https address.");
        }

        return url;
    }

    /// <summary>
    /// Builds a short, human readable code from a prefix and a new identifier.
    /// </summary>
    /// <param name="prefix">Prefix of the code, for example <c>INC</c>.</param>
    /// <param name="id">The identifier the code is derived from.</param>
    /// <returns>A code such as <c>INC-1A2B3C4D</c>.</returns>
    public static string CodeFrom(string prefix, string id)
    {
        return $"{prefix}-{id.Replace("-", string.Empty)[..8].ToUpperInvariant()}";
    }
}
