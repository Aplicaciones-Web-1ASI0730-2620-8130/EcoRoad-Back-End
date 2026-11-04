namespace EcoRoad.Iam.Domain.Model.Commands;

/// <summary>
/// Revokes the platform access of a collaborator.
/// </summary>
/// <param name="ActorUserId">Identifier of the acting administrator.</param>
/// <param name="UserId">Identifier of the collaborator.</param>
public record RevokeUserAccessCommand(string ActorUserId, string UserId);
