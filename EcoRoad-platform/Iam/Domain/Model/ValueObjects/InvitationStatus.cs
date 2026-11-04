namespace EcoRoad.Iam.Domain.Model.ValueObjects;

/// <summary>
/// Lifecycle of an invitation sent to a new collaborator.
/// </summary>
public enum InvitationStatus
{
    /// <summary>The invitation was sent and can still be accepted.</summary>
    Pending = 1,

    /// <summary>The invited person accepted and a user account was created.</summary>
    Accepted = 2,

    /// <summary>The invitation was not accepted before its expiration date.</summary>
    Expired = 3
}
