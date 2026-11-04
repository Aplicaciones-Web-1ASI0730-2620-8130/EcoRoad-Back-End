using EcoRoad.Iam.Domain.Model.Aggregates;
using EcoRoad.Iam.Domain.Model.Commands;

namespace EcoRoad.Iam.Application.CommandServices;

/// <summary>
/// Application service that handles the commands of the <see cref="User" /> aggregate.
/// </summary>
public interface IUserCommandService
{
    /// <summary>Registers the first administrator of a company.</summary>
    /// <param name="command">The sign up command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The new administrator.</returns>
    Task<User> Handle(SignUpCompanyAdministratorCommand command, CancellationToken cancellationToken = default);

    /// <summary>Authenticates a collaborator and issues a session token.</summary>
    /// <param name="command">The sign in command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The authenticated session.</returns>
    Task<AuthenticatedSession> Handle(SignInCommand command, CancellationToken cancellationToken = default);

    /// <summary>Grants platform access to a collaborator.</summary>
    /// <param name="command">The grant command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The updated collaborator.</returns>
    Task<User> Handle(GrantUserAccessCommand command, CancellationToken cancellationToken = default);

    /// <summary>Revokes the platform access of a collaborator.</summary>
    /// <param name="command">The revoke command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The updated collaborator.</returns>
    Task<User> Handle(RevokeUserAccessCommand command, CancellationToken cancellationToken = default);

    /// <summary>Assigns a new role to a collaborator.</summary>
    /// <param name="command">The role assignment command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The updated collaborator.</returns>
    Task<User> Handle(AssignUserRoleCommand command, CancellationToken cancellationToken = default);

    /// <summary>Replaces the permissions of a collaborator.</summary>
    /// <param name="command">The permissions update command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The updated collaborator.</returns>
    Task<User> Handle(UpdateUserPermissionsCommand command, CancellationToken cancellationToken = default);
}
