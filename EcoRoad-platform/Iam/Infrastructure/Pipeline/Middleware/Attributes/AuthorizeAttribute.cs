using EcoRoad.Iam.Infrastructure.Pipeline.Middleware.Extensions;
using EcoRoad.Shared.Interfaces.Rest.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EcoRoad.Iam.Infrastructure.Pipeline.Middleware.Attributes;

/// <summary>
/// Requires an authenticated user with active access. Responds with <c>401</c> otherwise.
/// </summary>
/// <remarks>
/// The authenticated user is resolved beforehand by the request authorization middleware.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    /// <inheritdoc />
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
        {
            return;
        }

        if (context.HttpContext.FindAuthenticatedUser() == null)
        {
            context.Result = new UnauthorizedObjectResult(
                new ErrorResource("UNAUTHENTICATED", "A valid session token is required."));
        }
    }
}
