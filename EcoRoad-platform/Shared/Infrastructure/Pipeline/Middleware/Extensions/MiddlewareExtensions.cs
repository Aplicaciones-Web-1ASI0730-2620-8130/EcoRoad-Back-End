using EcoRoad.Shared.Infrastructure.Pipeline.Middleware.Components;

namespace EcoRoad.Shared.Infrastructure.Pipeline.Middleware.Extensions;

/// <summary>
/// Registration helpers for the shared HTTP middlewares.
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Adds <see cref="GlobalExceptionHandlerMiddleware" /> to the pipeline. It must be the first middleware.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The same application builder, to allow chaining.</returns>
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}
