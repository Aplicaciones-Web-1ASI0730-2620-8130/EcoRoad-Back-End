namespace EcoRoad.Iam.Domain.Model.Commands;

/// <summary>
/// Accepts an invitation and creates the account of the invited person.
/// </summary>
/// <param name="InvitationId">Identifier of the invitation.</param>
/// <param name="Password">Password chosen by the invited person.</param>
public record AcceptInvitationCommand(string InvitationId, string Password);
