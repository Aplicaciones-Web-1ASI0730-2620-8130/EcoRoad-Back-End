using EcoRoad.Iam.Infrastructure.Pipeline.Middleware.Components;

namespace EcoRoad.Iam.Infrastructure.Pipeline.Middleware.Extensions;

/// <summary>
/// Registration helper for <see cref="RequestAuthorizationMiddleware" />.
/// </summary>
public static class RequestAuthorizationMiddlewareExtensions
{
    /// <summary>
    /// Adds the request authorization middleware to the pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The same application builder, to allow chaining.</returns>
    public static IApplicationBuilder UseRequestAuthorization(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestAuthorizationMiddleware>();
    }
}
