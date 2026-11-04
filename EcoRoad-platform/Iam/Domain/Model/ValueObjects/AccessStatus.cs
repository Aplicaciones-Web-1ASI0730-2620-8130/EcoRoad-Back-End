namespace EcoRoad.Iam.Domain.Model.ValueObjects;

/// <summary>
/// Lifecycle of the platform access of a collaborator.
/// </summary>
public enum AccessStatus
{
    /// <summary>The collaborator exists but an administrator has not granted access yet.</summary>
    Pending = 1,

    /// <summary>The collaborator can sign in and operate the platform.</summary>
    Active = 2,

    /// <summary>An administrator revoked the access; any existing session becomes invalid.</summary>
    Revoked = 3
}
