using EcoRoad.Iam.Domain.Model.Commands;
using EcoRoad.Iam.Interfaces.Rest.Resources;

namespace EcoRoad.Iam.Interfaces.Rest.Transform;

/// <summary>
/// Converts IAM request resources into domain commands.
/// </summary>
public static class IamCommandFromResourceAssembler
{
    /// <summary>Builds the sign up command.</summary>
    /// <param name="resource">The sign up request.</param>
    /// <returns>The command.</returns>
    public static SignUpCompanyAdministratorCommand ToCommandFromResource(SignUpResource resource)
    {
        return new SignUpCompanyAdministratorCommand(resource.CompanyId, resource.Name, resource.Email, resource.Password);
    }

    /// <summary>Builds the sign in command.</summary>
    /// <param name="resource">The sign in request.</param>
    /// <returns>The command.</returns>
    public static SignInCommand ToCommandFromResource(SignInResource resource)
    {
        return new SignInCommand(resource.CompanyId, resource.Email, resource.Password);
    }

    /// <summary>Builds the invitation command.</summary>
    /// <param name="actorUserId">Identifier of the acting administrator.</param>
    /// <param name="resource">The invitation request.</param>
    /// <returns>The command.</returns>
    public static InviteUserCommand ToCommandFromResource(string actorUserId, InviteUserResource resource)
    {
        return new InviteUserCommand(actorUserId, resource.Name, resource.Email, resource.Role);
    }

    /// <summary>Builds the invitation acceptance command.</summary>
    /// <param name="invitationId">Identifier of the invitation.</param>
    /// <param name="resource">The acceptance request.</param>
    /// <returns>The command.</returns>
    public static AcceptInvitationCommand ToCommandFromResource(string invitationId, AcceptInvitationResource resource)
    {
        return new AcceptInvitationCommand(invitationId, resource.Password);
    }

    /// <summary>Builds the role assignment command.</summary>
    /// <param name="actorUserId">Identifier of the acting administrator.</param>
    /// <param name="userId">Identifier of the collaborator.</param>
    /// <param name="resource">The role request.</param>
    /// <returns>The command.</returns>
    public static AssignUserRoleCommand ToCommandFromResource(string actorUserId, string userId, AssignRoleResource resource)
    {
        return new AssignUserRoleCommand(actorUserId, userId, resource.Role);
    }

    /// <summary>Builds the permissions update command.</summary>
    /// <param name="actorUserId">Identifier of the acting administrator.</param>
    /// <param name="userId">Identifier of the collaborator.</param>
    /// <param name="resource">The permissions request.</param>
    /// <returns>The command.</returns>
    public static UpdateUserPermissionsCommand ToCommandFromResource(
        string actorUserId,
        string userId,
        UpdatePermissionsResource resource)
    {
        return new UpdateUserPermissionsCommand(actorUserId, userId, resource.Permissions);
    }
}
