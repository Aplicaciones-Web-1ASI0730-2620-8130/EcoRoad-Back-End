using EcoRoad.Iam.Application.Internal.OutboundServices;
using EcoRoad.Iam.Application.QueryServices;
using EcoRoad.Iam.Domain.Model.Queries;
using EcoRoad.Iam.Infrastructure.Pipeline.Middleware.Extensions;

namespace EcoRoad.Iam.Infrastructure.Pipeline.Middleware.Components;

/// <summary>
/// Resolves the user of the bearer token sent with each request and stores it in the HTTP context.
/// </summary>
/// <remarks>
/// The middleware never rejects a request by itself, so endpoints of other bounded contexts keep
/// working unchanged. Endpoints decorated with <c>[Authorize]</c> reject requests without a user.
/// Users whose access was revoked are ignored, which invalidates their existing sessions.
/// </remarks>
public class RequestAuthorizationMiddleware
{
    private const string BearerPrefix = "Bearer ";

    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestAuthorizationMiddleware" /> class.
    /// </summary>
    /// <param name="next">The next middleware of the pipeline.</param>
    public RequestAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Validates the bearer token, when present, and attaches the authenticated user.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <param name="tokenService">Service used to validate the token.</param>
    /// <param name="userQueryService">Query service used to load the user.</param>
    public async Task InvokeAsync(
        HttpContext context,
        ITokenService tokenService,
        IUserQueryService userQueryService)
    {
        var header = context.Request.Headers.Authorization.FirstOrDefault();
        if (header != null && header.StartsWith(BearerPrefix, StringComparison.OrdinalIgnoreCase))
        {
            var userId = await tokenService.ValidateTokenAsync(header[BearerPrefix.Length..].Trim());
            if (userId != null)
            {
                var user = await userQueryService.Handle(new GetUserByIdQuery(userId), context.RequestAborted);
                if (user is { CanSignIn: true })
                {
                    context.SetAuthenticatedUser(user);
                }
            }
        }

        await _next(context);
    }
}
