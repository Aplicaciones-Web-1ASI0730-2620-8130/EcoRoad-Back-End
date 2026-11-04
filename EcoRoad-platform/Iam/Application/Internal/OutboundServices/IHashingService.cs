namespace EcoRoad.Iam.Application.Internal.OutboundServices;

/// <summary>
/// Outbound port used to hash and verify passwords.
/// </summary>
public interface IHashingService
{
    /// <summary>
    /// Hashes a plain text password.
    /// </summary>
    /// <param name="password">The plain text password.</param>
    /// <returns>The password hash.</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verifies a plain text password against a stored hash.
    /// </summary>
    /// <param name="password">The plain text password.</param>
    /// <param name="passwordHash">The stored hash.</param>
    /// <returns><c>true</c> when the password matches the hash.</returns>
    bool VerifyPassword(string password, string passwordHash);
}
