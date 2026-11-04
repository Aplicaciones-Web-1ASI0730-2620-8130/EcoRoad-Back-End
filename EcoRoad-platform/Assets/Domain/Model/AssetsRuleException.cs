using EcoRoad.Shared.Domain.Model.Exceptions;

namespace EcoRoad.Assets.Domain.Model;

/// <summary>
/// Raised when a rule of the Monitoring Asset and Deployment bounded context is broken.
/// </summary>
public class AssetsRuleException : DomainRuleException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AssetsRuleException" /> class.
    /// </summary>
    /// <param name="code">Stable error code, for example <c>SENSOR_NOT_FOUND</c>.</param>
    /// <param name="message">Human readable description of the broken rule.</param>
    /// <param name="kind">Category of the error; defaults to a validation error.</param>
    public AssetsRuleException(string code, string message, DomainErrorKind kind = DomainErrorKind.Validation)
        : base(code, message, kind)
    {
    }
}
