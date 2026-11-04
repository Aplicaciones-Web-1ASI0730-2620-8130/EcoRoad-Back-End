using EcoRoad.Shared.Domain.Model.Exceptions;
using EcoRoad.Shared.Interfaces.Rest.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EcoRoad.Shared.Interfaces.Rest.Filters;

/// <summary>
/// MVC exception filter that translates any <see cref="DomainRuleException" /> into an
/// <see cref="ErrorResource" /> with the matching HTTP status code.
/// </summary>
/// <remarks>
/// Registering this filter globally keeps controllers free of repetitive try/catch blocks.
/// </remarks>
public class DomainRuleExceptionFilter : IExceptionFilter
{
    private readonly ILogger<DomainRuleExceptionFilter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainRuleExceptionFilter" /> class.
    /// </summary>
    /// <param name="logger">Logger used to trace the rejected requests.</param>
    public DomainRuleExceptionFilter(ILogger<DomainRuleExceptionFilter> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not DomainRuleException exception)
        {
            return;
        }

        _logger.LogInformation(
            "Request {Path} rejected with {Code}: {Message}",
            context.HttpContext.Request.Path,
            exception.Code,
            exception.Message);

        context.Result = new ObjectResult(new ErrorResource(exception.Code, exception.Message))
        {
            StatusCode = ToStatusCode(exception.Kind)
        };
        context.ExceptionHandled = true;
    }

    /// <summary>
    /// Maps a domain error category to its HTTP status code.
    /// </summary>
    /// <param name="kind">The category of the broken rule.</param>
    /// <returns>The HTTP status code to return.</returns>
    private static int ToStatusCode(DomainErrorKind kind)
    {
        return kind switch
        {
            DomainErrorKind.NotFound => StatusCodes.Status404NotFound,
            DomainErrorKind.Conflict => StatusCodes.Status409Conflict,
            DomainErrorKind.Forbidden => StatusCodes.Status403Forbidden,
            DomainErrorKind.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status422UnprocessableEntity
        };
    }
}
