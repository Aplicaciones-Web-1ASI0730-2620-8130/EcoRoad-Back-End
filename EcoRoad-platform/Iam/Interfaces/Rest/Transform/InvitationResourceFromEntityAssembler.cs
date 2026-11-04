using EcoRoad.Iam.Domain.Model.Aggregates;
using EcoRoad.Iam.Interfaces.Rest.Resources;

namespace EcoRoad.Iam.Interfaces.Rest.Transform;

/// <summary>
/// Converts <see cref="UserInvitation" /> aggregates into <see cref="InvitationResource" /> objects.
/// </summary>
public static class InvitationResourceFromEntityAssembler
{
    /// <summary>
    /// Converts an invitation into its resource.
    /// </summary>
    /// <param name="invitation">The invitation to convert.</param>
    /// <returns>The invitation resource.</returns>
    public static InvitationResource ToResourceFromEntity(UserInvitation invitation)
    {
        return new InvitationResource(
            invitation.Id,
            invitation.CompanyId,
            invitation.Name,
            invitation.Email,
            invitation.Role,
            invitation.Status,
            invitation.InvitedByUserId,
            invitation.ExpiresAt,
            invitation.AcceptedAt,
            invitation.CreatedAt);
    }

    /// <summary>
    /// Converts a list of invitations into resources.
    /// </summary>
    /// <param name="invitations">The invitations to convert.</param>
    /// <returns>The invitation resources.</returns>
    public static IReadOnlyList<InvitationResource> ToResourcesFromEntities(IEnumerable<UserInvitation> invitations)
    {
        return invitations.Select(ToResourceFromEntity).ToList();
    }
}
