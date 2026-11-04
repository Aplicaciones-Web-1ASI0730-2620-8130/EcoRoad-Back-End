using EcoRoad.Iam.Domain.Model.ValueObjects;

namespace EcoRoad.Iam.Domain.Model.Commands;

/// <summary>
/// Assigns a new role to a collaborator, resetting its permissions to the role defaults.
/// </summary>
/// <param name="ActorUserId">Identifier of the acting administrator.</param>
/// <param name="UserId">Identifier of the collaborator.</param>
/// <param name="Role">The role to assign.</param>
public record AssignUserRoleCommand(string ActorUserId, string UserId, Role Role);
