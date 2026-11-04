using EcoRoad.Iam.Domain.Model.ValueObjects;
using EcoRoad.Shared.Domain.Model.Entities;
using EcoRoad.Shared.Domain.Model.Exceptions;

namespace EcoRoad.Iam.Domain.Model.Aggregates;

/// <summary>
/// Collaborator of a company that can sign in to EcoRoad once an administrator grants access.
/// </summary>
/// <remarks>
/// The aggregate owns the role, the permissions and the access status of the collaborator,
/// so every authorization decision of the platform is taken from a single consistent source.
/// </remarks>
public class User : IAuditableEntity
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    private User()
    {
    }

    /// <summary>
    /// Initializes a new collaborator with the default permissions of its role.
    /// </summary>
    /// <param name="companyId">Identifier of the company the collaborator belongs to.</param>
    /// <param name="name">Full name of the collaborator.</param>
    /// <param name="email">Email address used to sign in.</param>
    /// <param name="passwordHash">Hash of the password; the plain text password is never stored.</param>
    /// <param name="role">Role assigned to the collaborator.</param>
    private User(string companyId, string name, string email, string passwordHash, Role role)
    {
        Id = Guid.NewGuid().ToString();
        CompanyId = IamRules.Require(companyId, "Company id", 36);
        Name = IamRules.Require(name, "Name", 100);
        Email = IamRules.RequireEmail(email);
        PasswordHash = IamRules.Require(passwordHash, "Password hash", 255);
        Role = IamRules.RequireDefined(role, "INVALID_ROLE", "Role");
        Permissions = RolePermissions.DefaultsFor(Role);
        AccessStatus = AccessStatus.Pending;
    }

    /// <summary>Gets the unique identifier of the user.</summary>
    public string Id { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the company the user belongs to.</summary>
    public string CompanyId { get; private set; } = string.Empty;

    /// <summary>Gets the full name of the user.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets the normalized email address used to sign in.</summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>Gets the hash of the user password.</summary>
    public string PasswordHash { get; private set; } = string.Empty;

    /// <summary>Gets the role of the user inside the company.</summary>
    public Role Role { get; private set; }

    /// <summary>Gets the effective permissions of the user.</summary>
    public IReadOnlyList<Permission> Permissions { get; private set; } = new List<Permission>();

    /// <summary>Gets the current access status of the user.</summary>
    public AccessStatus AccessStatus { get; private set; }

    /// <summary>Gets the UTC date when access was last granted, if ever.</summary>
    public DateTime? GrantedAt { get; private set; }

    /// <summary>Gets the UTC date when access was last revoked, if ever.</summary>
    public DateTime? RevokedAt { get; private set; }

    /// <inheritdoc />
    public DateTime? CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets a value indicating whether the user is allowed to sign in.
    /// </summary>
    public bool CanSignIn => AccessStatus == AccessStatus.Active;

    /// <summary>
    /// Registers the first administrator of a company. Its access is active immediately.
    /// </summary>
    /// <param name="companyId">Identifier of the company being onboarded.</param>
    /// <param name="name">Full name of the administrator.</param>
    /// <param name="email">Email address used to sign in.</param>
    /// <param name="passwordHash">Hash of the administrator password.</param>
    /// <returns>The new administrator.</returns>
    public static User RegisterCompanyAdministrator(string companyId, string name, string email, string passwordHash)
    {
        var administrator = new User(companyId, name, email, passwordHash, Role.CompanyAdmin);
        administrator.GrantAccess(DateTime.UtcNow);
        return administrator;
    }

    /// <summary>
    /// Creates the user generated when an invitation is accepted. Its access stays pending
    /// until an administrator grants it.
    /// </summary>
    /// <param name="invitation">The accepted invitation.</param>
    /// <param name="passwordHash">Hash of the password chosen by the invited person.</param>
    /// <returns>The new user with pending access.</returns>
    public static User FromAcceptedInvitation(UserInvitation invitation, string passwordHash)
    {
        if (invitation.Status != InvitationStatus.Accepted)
        {
            throw new IamRuleException(
                "INVITATION_NOT_ACCEPTED",
                "Only an accepted invitation can generate a user.",
                DomainErrorKind.Conflict);
        }

        return new User(invitation.CompanyId, invitation.Name, invitation.Email, passwordHash, invitation.Role);
    }

    /// <summary>
    /// Checks whether the user holds a permission.
    /// </summary>
    /// <param name="permission">The permission to check.</param>
    /// <returns><c>true</c> when the user has the permission and active access.</returns>
    public bool HasPermission(Permission permission)
    {
        return CanSignIn && Permissions.Contains(permission);
    }

    /// <summary>
    /// Grants platform access to the user.
    /// </summary>
    /// <param name="now">Current UTC date and time.</param>
    public void GrantAccess(DateTime now)
    {
        if (AccessStatus == AccessStatus.Active)
        {
            throw new IamRuleException(
                "ALREADY_ACTIVE",
                "The collaborator already has active access.",
                DomainErrorKind.Conflict);
        }

        AccessStatus = AccessStatus.Active;
        GrantedAt = now;
        RevokedAt = null;
    }

    /// <summary>
    /// Revokes the platform access of the user. Existing sessions stop being accepted.
    /// </summary>
    /// <param name="now">Current UTC date and time.</param>
    public void RevokeAccess(DateTime now)
    {
        if (AccessStatus != AccessStatus.Active)
        {
            throw new IamRuleException(
                "ACCESS_NOT_ACTIVE",
                "The collaborator does not have active access.",
                DomainErrorKind.Conflict);
        }

        AccessStatus = AccessStatus.Revoked;
        RevokedAt = now;
    }

    /// <summary>
    /// Assigns a new role and resets the permissions to the defaults of that role.
    /// </summary>
    /// <param name="role">The role to assign.</param>
    public void AssignRole(Role role)
    {
        Role = IamRules.RequireDefined(role, "INVALID_ROLE", "Role");
        Permissions = RolePermissions.DefaultsFor(Role);
    }

    /// <summary>
    /// Replaces the effective permissions of the user. Requires active access.
    /// </summary>
    /// <param name="permissions">The new set of permissions.</param>
    public void UpdatePermissions(IEnumerable<Permission> permissions)
    {
        if (AccessStatus != AccessStatus.Active)
        {
            throw new IamRuleException(
                "ACCESS_NOT_ACTIVE",
                "Grant access before editing the permissions.",
                DomainErrorKind.Conflict);
        }

        var distinct = permissions.Distinct().ToList();
        if (distinct.Any(permission => !Enum.IsDefined(permission)))
        {
            throw new IamRuleException("INVALID_PERMISSIONS", "The selection contains unknown permissions.");
        }

        Permissions = distinct.OrderBy(permission => permission).ToList();
    }
}
