using EcoRoad.Iam.Domain.Model.ValueObjects;
using EcoRoad.Shared.Domain.Model.Entities;
using EcoRoad.Shared.Domain.Model.Exceptions;

namespace EcoRoad.Iam.Domain.Model.Aggregates;

/// <summary>
/// Invitation sent by a company administrator to a new collaborator.
/// </summary>
/// <remarks>
/// Accepting the invitation generates a <see cref="User" /> whose access stays pending
/// until an administrator explicitly grants it.
/// </remarks>
public class UserInvitation : IAuditableEntity
{
    /// <summary>Number of days an invitation remains valid.</summary>
    public const int ValidityInDays = 7;

    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    private UserInvitation()
    {
    }

    /// <summary>
    /// Initializes a new pending invitation.
    /// </summary>
    /// <param name="companyId">Identifier of the inviting company.</param>
    /// <param name="name">Full name of the invited person.</param>
    /// <param name="email">Email address of the invited person.</param>
    /// <param name="role">Role the person will receive.</param>
    /// <param name="invitedByUserId">Identifier of the administrator that sends the invitation.</param>
    /// <param name="now">Current UTC date and time.</param>
    public UserInvitation(
        string companyId,
        string name,
        string email,
        Role role,
        string invitedByUserId,
        DateTime now)
    {
        Id = Guid.NewGuid().ToString();
        CompanyId = IamRules.Require(companyId, "Company id", 36);
        Name = IamRules.Require(name, "Name", 100);
        Email = IamRules.RequireEmail(email);
        Role = IamRules.RequireDefined(role, "INVALID_ROLE", "Role");
        InvitedByUserId = IamRules.Require(invitedByUserId, "Invited by", 36);
        Status = InvitationStatus.Pending;
        ExpiresAt = now.AddDays(ValidityInDays);
    }

    /// <summary>Gets the unique identifier of the invitation.</summary>
    public string Id { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the inviting company.</summary>
    public string CompanyId { get; private set; } = string.Empty;

    /// <summary>Gets the full name of the invited person.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets the normalized email address of the invited person.</summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>Gets the role the person will receive.</summary>
    public Role Role { get; private set; }

    /// <summary>Gets the current status of the invitation.</summary>
    public InvitationStatus Status { get; private set; }

    /// <summary>Gets the identifier of the administrator that sent the invitation.</summary>
    public string InvitedByUserId { get; private set; } = string.Empty;

    /// <summary>Gets the UTC date after which the invitation can no longer be accepted.</summary>
    public DateTime ExpiresAt { get; private set; }

    /// <summary>Gets the UTC date when the invitation was accepted, if it was.</summary>
    public DateTime? AcceptedAt { get; private set; }

    /// <inheritdoc />
    public DateTime? CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Checks whether the invitation passed its expiration date.
    /// </summary>
    /// <param name="now">Current UTC date and time.</param>
    /// <returns><c>true</c> when the invitation can no longer be accepted.</returns>
    public bool IsExpired(DateTime now)
    {
        return Status == InvitationStatus.Expired
            || Status == InvitationStatus.Pending && now > ExpiresAt;
    }

    /// <summary>
    /// Marks a pending invitation as expired once its validity is over.
    /// </summary>
    /// <param name="now">Current UTC date and time.</param>
    /// <returns><c>true</c> when the status changed.</returns>
    public bool ExpireIfOverdue(DateTime now)
    {
        if (Status != InvitationStatus.Pending || now <= ExpiresAt)
        {
            return false;
        }

        Status = InvitationStatus.Expired;
        return true;
    }

    /// <summary>
    /// Accepts the invitation.
    /// </summary>
    /// <param name="now">Current UTC date and time.</param>
    public void Accept(DateTime now)
    {
        if (IsExpired(now))
        {
            throw new IamRuleException(
                "INVITATION_EXPIRED",
                "The invitation has expired. Ask the administrator for a new one.",
                DomainErrorKind.Conflict);
        }

        if (Status != InvitationStatus.Pending)
        {
            throw new IamRuleException(
                "INVITATION_NOT_PENDING",
                "The invitation was already used.",
                DomainErrorKind.Conflict);
        }

        Status = InvitationStatus.Accepted;
        AcceptedAt = now;
    }
}
