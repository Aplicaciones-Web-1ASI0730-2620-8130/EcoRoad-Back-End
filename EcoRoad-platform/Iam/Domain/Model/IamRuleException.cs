using EcoRoad.Shared.Domain.Model.Exceptions;

namespace EcoRoad.Iam.Domain.Model;

/// <summary>
/// Raised when a rule of the Identity and Access Management bounded context is broken.
/// </summary>
public class IamRuleException : DomainRuleException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IamRuleException" /> class.
    /// </summary>
    /// <param name="code">Stable error code, for example <c>USER_NOT_FOUND</c>.</param>
    /// <param name="message">Human readable description of the broken rule.</param>
    /// <param name="kind">Category of the error; defaults to a validation error.</param>
    public IamRuleException(string code, string message, DomainErrorKind kind = DomainErrorKind.Validation)
        : base(code, message, kind)
    {
    }
}
