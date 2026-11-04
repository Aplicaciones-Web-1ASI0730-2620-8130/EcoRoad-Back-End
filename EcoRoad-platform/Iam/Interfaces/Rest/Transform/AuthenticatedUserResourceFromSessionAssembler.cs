using EcoRoad.Iam.Application.CommandServices;
using EcoRoad.Iam.Interfaces.Rest.Resources;

namespace EcoRoad.Iam.Interfaces.Rest.Transform;

/// <summary>
/// Converts an <see cref="AuthenticatedSession" /> into an <see cref="AuthenticatedUserResource" />.
/// </summary>
public static class AuthenticatedUserResourceFromSessionAssembler
{
    /// <summary>
    /// Converts a session into its resource.
    /// </summary>
    /// <param name="session">The authenticated session.</param>
    /// <returns>The session resource.</returns>
    public static AuthenticatedUserResource ToResourceFromSession(AuthenticatedSession session)
    {
        return new AuthenticatedUserResource(
            session.Token,
            UserResourceFromEntityAssembler.ToResourceFromEntity(session.User));
    }
}
