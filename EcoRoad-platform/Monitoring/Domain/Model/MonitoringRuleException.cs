using EcoRoad.Shared.Domain.Model.Exceptions;

namespace EcoRoad.Monitoring.Domain.Model;

/// <summary>
/// Raised when a rule of the Environmental Monitoring bounded context is broken.
/// </summary>
public class MonitoringRuleException : DomainRuleException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MonitoringRuleException" /> class.
    /// </summary>
    /// <param name="code">Stable error code, for example <c>INVALID_MEASUREMENT</c>.</param>
    /// <param name="message">Human readable description of the broken rule.</param>
    /// <param name="kind">Category of the error; defaults to a validation error.</param>
    public MonitoringRuleException(string code, string message, DomainErrorKind kind = DomainErrorKind.Validation)
        : base(code, message, kind)
    {
    }
}
