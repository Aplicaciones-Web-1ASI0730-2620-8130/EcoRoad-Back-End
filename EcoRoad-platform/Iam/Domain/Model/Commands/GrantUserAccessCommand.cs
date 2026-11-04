namespace EcoRoad.Iam.Domain.Model.Commands;

/// <summary>
/// Grants platform access to a collaborator.
/// </summary>
/// <param name="ActorUserId">Identifier of the acting administrator.</param>
/// <param name="UserId">Identifier of the collaborator.</param>
public record GrantUserAccessCommand(string ActorUserId, string UserId);
