using EcoRoad.Iam.Domain.Model.Aggregates;

namespace EcoRoad.Iam.Application.CommandServices;

/// <summary>
/// Result of a successful sign in.
/// </summary>
/// <param name="User">The authenticated user.</param>
/// <param name="Token">The signed session token the client must send as a bearer token.</param>
public record AuthenticatedSession(User User, string Token);
