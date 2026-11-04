using EcoRoad.Shared.Domain.Model.Exceptions;

namespace EcoRoad.Incidents.Domain.Model;

/// <summary>
/// Raised when a rule of the Incident and Remediation Management bounded context is broken.
/// </summary>
public class IncidentsRuleException : DomainRuleException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IncidentsRuleException" /> class.
    /// </summary>
    /// <param name="code">Stable error code, for example <c>INCIDENT_NOT_FOUND</c>.</param>
    /// <param name="message">Human readable description of the broken rule.</param>
    /// <param name="kind">Category of the error; defaults to a validation error.</param>
    public IncidentsRuleException(string code, string message, DomainErrorKind kind = DomainErrorKind.Validation)
        : base(code, message, kind)
    {
    }
}
