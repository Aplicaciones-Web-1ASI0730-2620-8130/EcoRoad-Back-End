using System.Globalization;
using System.Text.RegularExpressions;

namespace EcoRoad.Assets.Domain.Model.ValueObjects;

/// <summary>
/// Kilometer post (PK) that locates a point along a road corridor, written as <c>KKK+MMM</c>,
/// for example <c>45+100</c> for kilometer 45 plus 100 meters.
/// </summary>
public sealed partial record KilometerPost
{
    private KilometerPost(int kilometers, int meters)
    {
        Kilometers = kilometers;
        Meters = meters;
    }

    /// <summary>Gets the whole kilometers of the post.</summary>
    public int Kilometers { get; }

    /// <summary>Gets the additional meters of the post, from 0 to 999.</summary>
    public int Meters { get; }

    /// <summary>Gets the distance from the origin of the corridor, in meters.</summary>
    public int TotalMeters => Kilometers * 1000 + Meters;

    /// <summary>
    /// Parses a kilometer post. Accepts an optional <c>PK</c> prefix and surrounding spaces.
    /// </summary>
    /// <param name="value">Text such as <c>PK 05+000</c> or <c>5+000</c>.</param>
    /// <param name="post">The parsed kilometer post.</param>
    /// <returns><c>true</c> when the text is a valid kilometer post.</returns>
    public static bool TryParse(string? value, out KilometerPost? post)
    {
        post = null;
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var match = Pattern().Match(value.Trim());
        if (!match.Success)
        {
            return false;
        }

        post = new KilometerPost(
            int.Parse(match.Groups["km"].Value, CultureInfo.InvariantCulture),
            int.Parse(match.Groups["m"].Value, CultureInfo.InvariantCulture));
        return true;
    }

    /// <summary>
    /// Returns the canonical representation, for example <c>05+000</c>.
    /// </summary>
    /// <returns>The kilometer post as text.</returns>
    public override string ToString()
    {
        return $"{Kilometers:00}+{Meters:000}";
    }

    [GeneratedRegex(@"^(?:PK\s*)?(?<km>\d{1,3})\+(?<m>\d{3})$", RegexOptions.IgnoreCase)]
    private static partial Regex Pattern();
}
