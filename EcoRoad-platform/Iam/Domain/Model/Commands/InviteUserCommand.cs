using EcoRoad.Iam.Domain.Model.ValueObjects;

namespace EcoRoad.Iam.Domain.Model.Commands;

/// <summary>
/// Invites a new collaborator to the company of the acting administrator.
/// </summary>
/// <param name="ActorUserId">Identifier of the administrator that sends the invitation.</param>
/// <param name="Name">Full name of the invited person.</param>
/// <param name="Email">Email address of the invited person.</param>
/// <param name="Role">Role the person will receive.</param>
public record InviteUserCommand(string ActorUserId, string Name, string Email, Role Role);
