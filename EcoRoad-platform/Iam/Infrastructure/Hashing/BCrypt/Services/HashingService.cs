using EcoRoad.Iam.Application.Internal.OutboundServices;
using BCryptNet = BCrypt.Net.BCrypt;

namespace EcoRoad.Iam.Infrastructure.Hashing.BCrypt.Services;

/// <summary>
/// BCrypt implementation of <see cref="IHashingService" />. BCrypt salts every hash and is
/// deliberately slow, which protects the stored passwords against brute force attacks.
/// </summary>
public class HashingService : IHashingService
{
    /// <inheritdoc />
    public string HashPassword(string password)
    {
        return BCryptNet.HashPassword(password);
    }

    /// <inheritdoc />
    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCryptNet.Verify(password, passwordHash);
    }
}
