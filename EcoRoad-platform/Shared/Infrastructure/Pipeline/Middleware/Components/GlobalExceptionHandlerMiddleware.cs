using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoad.Shared.Infrastructure.Pipeline.Middleware.Components;

/// <summary>
/// Last line of defense of the HTTP pipeline: converts unhandled exceptions into
/// RFC 7807 problem details responses instead of leaking stack traces.
/// </summary>
public class GlobalExceptionHandlerMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalExceptionHandlerMiddleware" /> class.
    /// </summary>
    /// <param name="next">The next middleware of the pipeline.</param>
    /// <param name="logger">Logger used to record the unhandled exceptions.</param>
    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Executes the next middleware and handles any exception it throws.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            _logger.LogWarning("Request {Path} was cancelled by the client.", context.Request.Path);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception while processing {Path}.", context.Request.Path);
            await WriteProblemAsync(context);
        }
    }

    /// <summary>
    /// Writes a generic internal server error problem details body.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    private static async Task WriteProblemAsync(HttpContext context)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Detail = "An unexpected error occurred while processing the request.",
            Instance = context.Request.Path
        };

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = MediaTypeNames.Application.ProblemJson;
        await context.Response.WriteAsync(JsonSerializer.Serialize(problem, JsonOptions));
    }
}
