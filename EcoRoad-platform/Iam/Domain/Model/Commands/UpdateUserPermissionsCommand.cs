using EcoRoad.Iam.Domain.Model.ValueObjects;

namespace EcoRoad.Iam.Domain.Model.Commands;

/// <summary>
/// Replaces the effective permissions of a collaborator.
/// </summary>
/// <param name="ActorUserId">Identifier of the acting administrator.</param>
/// <param name="UserId">Identifier of the collaborator.</param>
/// <param name="Permissions">The new set of permissions.</param>
public record UpdateUserPermissionsCommand(string ActorUserId, string UserId, IReadOnlyList<Permission> Permissions);
