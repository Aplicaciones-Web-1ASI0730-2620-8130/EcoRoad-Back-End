using EcoRoad.Iam.Domain.Model;
using EcoRoad.Iam.Domain.Model.Aggregates;
using EcoRoad.Shared.Domain.Model.Exceptions;

namespace EcoRoad.Iam.Infrastructure.Pipeline.Middleware.Extensions;

/// <summary>
/// Helpers to store and read the authenticated user of the current request.
/// </summary>
public static class HttpContextUserExtensions
{
    private const string UserItemKey = "EcoRoad.Iam.AuthenticatedUser";

    /// <summary>
    /// Stores the authenticated user of the current request.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <param name="user">The authenticated user.</param>
    public static void SetAuthenticatedUser(this HttpContext context, User user)
    {
        context.Items[UserItemKey] = user;
    }

    /// <summary>
    /// Reads the authenticated user of the current request, if any.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>The authenticated user, or <c>null</c> for anonymous requests.</returns>
    public static User? FindAuthenticatedUser(this HttpContext context)
    {
        return context.Items.TryGetValue(UserItemKey, out var user) ? user as User : null;
    }

    /// <summary>
    /// Reads the authenticated user of the current request.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>The authenticated user.</returns>
    /// <exception cref="IamRuleException">Thrown when the request has no authenticated user.</exception>
    public static User GetAuthenticatedUser(this HttpContext context)
    {
        return context.FindAuthenticatedUser()
            ?? throw new IamRuleException(
                "UNAUTHENTICATED",
                "A valid session token is required.",
                DomainErrorKind.Unauthorized);
    }
}
