using EcoRoad.Iam.Domain.Model.ValueObjects;

namespace EcoRoad.Iam.Interfaces.Rest.Resources;

/// <summary>
/// Request body to register the first administrator of a company.
/// </summary>
/// <param name="CompanyId">Identifier of the company account.</param>
/// <param name="Name">Full name of the administrator.</param>
/// <param name="Email">Email address used to sign in.</param>
/// <param name="Password">Password with at least 8 characters, upper case, lower case and digits.</param>
public record SignUpResource(string CompanyId, string Name, string Email, string Password);

/// <summary>
/// Request body to open a session.
/// </summary>
/// <param name="CompanyId">Optional company identifier; required when the email belongs to several companies.</param>
/// <param name="Email">Email address of the collaborator.</param>
/// <param name="Password">Password of the collaborator.</param>
public record SignInResource(string? CompanyId, string Email, string Password);

/// <summary>
/// Request body to invite a collaborator.
/// </summary>
/// <param name="Name">Full name of the invited person.</param>
/// <param name="Email">Email address of the invited person.</param>
/// <param name="Role">Role the person will receive.</param>
public record InviteUserResource(string Name, string Email, Role Role);

/// <summary>
/// Request body to accept an invitation.
/// </summary>
/// <param name="Password">Password chosen by the invited person.</param>
public record AcceptInvitationResource(string Password);

/// <summary>
/// Request body to assign a role.
/// </summary>
/// <param name="Role">The role to assign.</param>
public record AssignRoleResource(Role Role);

/// <summary>
/// Request body to replace the permissions of a collaborator.
/// </summary>
/// <param name="Permissions">The new set of permissions.</param>
public record UpdatePermissionsResource(IReadOnlyList<Permission> Permissions);

/// <summary>
/// Collaborator returned by the API. The password hash is never exposed.
/// </summary>
/// <param name="Id">Identifier of the user.</param>
/// <param name="CompanyId">Identifier of the company.</param>
/// <param name="Name">Full name.</param>
/// <param name="Email">Email address.</param>
/// <param name="Role">Role inside the company.</param>
/// <param name="Permissions">Effective permissions.</param>
/// <param name="AccessStatus">Current access status.</param>
/// <param name="GrantedAt">UTC date when access was last granted.</param>
/// <param name="RevokedAt">UTC date when access was last revoked.</param>
/// <param name="CreatedAt">UTC date when the account was created.</param>
public record UserResource(
    string Id,
    string CompanyId,
    string Name,
    string Email,
    Role Role,
    IReadOnlyList<Permission> Permissions,
    AccessStatus AccessStatus,
    DateTime? GrantedAt,
    DateTime? RevokedAt,
    DateTime? CreatedAt);

/// <summary>
/// Session returned after a successful sign in.
/// </summary>
/// <param name="Token">Bearer token to send in the <c>Authorization</c> header.</param>
/// <param name="User">The authenticated collaborator.</param>
public record AuthenticatedUserResource(string Token, UserResource User);

/// <summary>
/// Invitation returned by the API.
/// </summary>
/// <param name="Id">Identifier of the invitation.</param>
/// <param name="CompanyId">Identifier of the inviting company.</param>
/// <param name="Name">Full name of the invited person.</param>
/// <param name="Email">Email address of the invited person.</param>
/// <param name="Role">Role the person will receive.</param>
/// <param name="Status">Current status of the invitation.</param>
/// <param name="InvitedByUserId">Identifier of the administrator that sent it.</param>
/// <param name="ExpiresAt">UTC expiration date.</param>
/// <param name="AcceptedAt">UTC date when it was accepted.</param>
/// <param name="CreatedAt">UTC date when it was sent.</param>
public record InvitationResource(
    string Id,
    string CompanyId,
    string Name,
    string Email,
    Role Role,
    InvitationStatus Status,
    string InvitedByUserId,
    DateTime ExpiresAt,
    DateTime? AcceptedAt,
    DateTime? CreatedAt);
