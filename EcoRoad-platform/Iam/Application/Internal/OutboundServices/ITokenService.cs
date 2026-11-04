using EcoRoad.Iam.Domain.Model.Aggregates;

namespace EcoRoad.Iam.Application.Internal.OutboundServices;

/// <summary>
/// Outbound port used to issue and validate session tokens.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Issues a signed session token for a user.
    /// </summary>
    /// <param name="user">The authenticated user.</param>
    /// <returns>The signed token.</returns>
    string GenerateToken(User user);

    /// <summary>
    /// Validates a session token.
    /// </summary>
    /// <param name="token">The token received from the client.</param>
    /// <returns>The identifier of the user when the token is valid; otherwise <c>null</c>.</returns>
    Task<string?> ValidateTokenAsync(string token);
}
