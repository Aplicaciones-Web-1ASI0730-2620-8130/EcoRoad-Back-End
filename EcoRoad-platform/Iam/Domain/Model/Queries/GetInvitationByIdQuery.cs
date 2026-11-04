namespace EcoRoad.Iam.Domain.Model.Queries;

/// <summary>
/// Retrieves an invitation by its identifier.
/// </summary>
/// <param name="InvitationId">Identifier of the invitation.</param>
public record GetInvitationByIdQuery(string InvitationId);
