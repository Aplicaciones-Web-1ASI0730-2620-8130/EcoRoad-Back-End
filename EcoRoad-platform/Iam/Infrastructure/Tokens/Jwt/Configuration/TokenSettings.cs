namespace EcoRoad.Iam.Infrastructure.Tokens.Jwt.Configuration;

/// <summary>
/// Settings used to sign and validate the JSON Web Tokens, bound from the <c>TokenSettings</c> section.
/// </summary>
public class TokenSettings
{
    /// <summary>Gets or sets the symmetric secret; it must contain at least 32 characters.</summary>
    public string Secret { get; set; } = string.Empty;

    /// <summary>Gets or sets the issuer written into every token.</summary>
    public string Issuer { get; set; } = "EcoRoad";

    /// <summary>Gets or sets how long a token remains valid, in hours.</summary>
    public int ExpirationInHours { get; set; } = 8;
}
