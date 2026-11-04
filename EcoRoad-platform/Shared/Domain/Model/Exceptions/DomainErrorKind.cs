namespace EcoRoad.Shared.Domain.Model.Exceptions;

/// <summary>
/// Classifies a broken business rule so the interface layer can translate it into a response.
/// </summary>
public enum DomainErrorKind
{
    /// <summary>The input data does not satisfy the domain invariants.</summary>
    Validation = 1,

    /// <summary>The requested resource does not exist.</summary>
    NotFound = 2,

    /// <summary>The operation conflicts with the current state of the resource.</summary>
    Conflict = 3,

    /// <summary>The caller is authenticated but not allowed to perform the operation.</summary>
    Forbidden = 4,

    /// <summary>The caller could not be authenticated.</summary>
    Unauthorized = 5
}
