using System.Text.RegularExpressions;
using EcoRoad.Assets.Domain.Model.ValueObjects;

namespace EcoRoad.Assets.Domain.Model;

/// <summary>
/// Reusable guard clauses that protect the invariants of the asset aggregates.
/// </summary>
internal static partial class AssetsRules
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
            throw new AssetsRuleException(code, $"{field} is required.");
        }

        var trimmed = value.Trim();
        if (trimmed.Length > maxLength)
        {
            throw new AssetsRuleException(code, $"{field} exceeds {maxLength} characters.");
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
    /// Ensures a kilometer post is well formed and returns its canonical representation.
    /// </summary>
    /// <param name="value">The kilometer post to validate.</param>
    /// <returns>The canonical kilometer post, for example <c>05+000</c>.</returns>
    public static string RequireKilometerPost(string? value)
    {
        if (!KilometerPost.TryParse(value, out var post))
        {
            throw new AssetsRuleException("INVALID_POINT", "Kilometer post must follow the format PK 00+000.");
        }

        return post!.ToString();
    }

    /// <summary>
    /// Ensures a pair of coordinates is valid.
    /// </summary>
    /// <param name="latitude">Latitude in decimal degrees.</param>
    /// <param name="longitude">Longitude in decimal degrees.</param>
    public static void RequireCoordinates(double latitude, double longitude)
    {
        if (!double.IsFinite(latitude) || latitude < -90 || latitude > 90)
        {
            throw new AssetsRuleException("INVALID_POINT", "Latitude must be between -90 and 90.");
        }

        if (!double.IsFinite(longitude) || longitude < -180 || longitude > 180)
        {
            throw new AssetsRuleException("INVALID_POINT", "Longitude must be between -180 and 180.");
        }
    }

    /// <summary>
    /// Ensures a serial number uses 3 to 40 letters, digits or hyphens and returns it in upper case.
    /// </summary>
    /// <param name="value">The serial number to validate.</param>
    /// <returns>The normalized serial number.</returns>
    public static string RequireSerialNumber(string? value)
    {
        var trimmed = value?.Trim().ToUpperInvariant() ?? string.Empty;
        if (!SerialPattern().IsMatch(trimmed))
        {
            throw new AssetsRuleException("INVALID_SENSOR", "Serial number must use 3 to 40 letters, digits or hyphens.");
        }

        return trimmed;
    }

    /// <summary>
    /// Ensures a sensor type is one of the supported types.
    /// </summary>
    /// <param name="type">The sensor type to validate.</param>
    /// <returns>The same sensor type when it is valid.</returns>
    public static SensorType RequireSensorType(SensorType type)
    {
        if (!Enum.IsDefined(type))
        {
            throw new AssetsRuleException("INVALID_SENSOR", "Sensor type is not supported.");
        }

        return type;
    }

    /// <summary>
    /// Builds a short, human readable code from a prefix and a new identifier.
    /// </summary>
    /// <param name="prefix">Prefix of the code, for example <c>SEN</c>.</param>
    /// <param name="id">The identifier the code is derived from.</param>
    /// <returns>A code such as <c>SEN-1A2B3C4D</c>.</returns>
    public static string CodeFrom(string prefix, string id)
    {
        return $"{prefix}-{id.Replace("-", string.Empty)[..8].ToUpperInvariant()}";
    }

    [GeneratedRegex("^[A-Z0-9-]{3,40}$")]
    private static partial Regex SerialPattern();
}
