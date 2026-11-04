using System.Security.Claims;
using System.Text;
using EcoRoad.Iam.Application.Internal.OutboundServices;
using EcoRoad.Iam.Domain.Model.Aggregates;
using EcoRoad.Iam.Infrastructure.Tokens.Jwt.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace EcoRoad.Iam.Infrastructure.Tokens.Jwt.Services;

/// <summary>
/// JSON Web Token implementation of <see cref="ITokenService" /> signed with HMAC SHA-256.
/// </summary>
public class TokenService : ITokenService
{
    /// <summary>Name of the claim that carries the company of the user.</summary>
    public const string CompanyIdClaim = "company_id";

    private readonly TokenSettings _settings;
    private readonly ILogger<TokenService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenService" /> class.
    /// </summary>
    /// <param name="settings">The token settings.</param>
    /// <param name="logger">Logger used to trace rejected tokens.</param>
    public TokenService(IOptions<TokenSettings> settings, ILogger<TokenService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public string GenerateToken(User user)
    {
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Sid, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(CompanyIdClaim, user.CompanyId)
            }),
            Issuer = _settings.Issuer,
            Expires = DateTime.UtcNow.AddHours(_settings.ExpirationInHours),
            SigningCredentials = new SigningCredentials(SigningKey(), SecurityAlgorithms.HmacSha256Signature)
        };

        return new JsonWebTokenHandler().CreateToken(descriptor);
    }

    /// <inheritdoc />
    public async Task<string?> ValidateTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        var result = await new JsonWebTokenHandler().ValidateTokenAsync(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SigningKey(),
            ValidateIssuer = true,
            ValidIssuer = _settings.Issuer,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        });

        if (!result.IsValid)
        {
            _logger.LogDebug("Rejected session token: {Reason}", result.Exception?.Message);
            return null;
        }

        return result.ClaimsIdentity.FindFirst(ClaimTypes.Sid)?.Value;
    }

    /// <summary>
    /// Builds the symmetric key used to sign and validate the tokens.
    /// </summary>
    /// <returns>The signing key.</returns>
    private SymmetricSecurityKey SigningKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
    }
}
