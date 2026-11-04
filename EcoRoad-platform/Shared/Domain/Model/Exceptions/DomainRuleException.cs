namespace EcoRoad.Shared.Domain.Model.Exceptions;

/// <summary>
/// Base exception raised when a business rule of any bounded context is broken.
/// </summary>
/// <remarks>
/// Each bounded context derives its own exception from this type so its rules stay
/// explicit, while the interface layer handles all of them in a single place.
/// </remarks>
public abstract class DomainRuleException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainRuleException" /> class.
    /// </summary>
    /// <param name="code">Stable, machine readable error code, for example <c>SENSOR_NOT_FOUND</c>.</param>
    /// <param name="message">Human readable description of the broken rule.</param>
    /// <param name="kind">Category used to choose the HTTP status code.</param>
    protected DomainRuleException(string code, string message, DomainErrorKind kind)
        : base(message)
    {
        Code = code;
        Kind = kind;
    }

    /// <summary>
    /// Gets the stable, machine readable error code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the category of the broken rule.
    /// </summary>
    public DomainErrorKind Kind { get; }
}
