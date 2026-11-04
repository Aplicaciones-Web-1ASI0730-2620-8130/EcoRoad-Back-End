using EcoRoad.Iam.Application.QueryServices;
using EcoRoad.Iam.Domain.Model.Aggregates;
using EcoRoad.Iam.Domain.Model.Queries;
using EcoRoad.Iam.Domain.Repositories;

namespace EcoRoad.Iam.Application.Internal.QueryServices;

/// <summary>
/// Answers the queries about invitations.
/// </summary>
public class UserInvitationQueryService : IUserInvitationQueryService
{
    private readonly IUserInvitationRepository _invitations;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserInvitationQueryService" /> class.
    /// </summary>
    /// <param name="invitations">Repository of invitations.</param>
    public UserInvitationQueryService(IUserInvitationRepository invitations)
    {
        _invitations = invitations;
    }

    /// <inheritdoc />
    public async Task<UserInvitation?> Handle(GetInvitationByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _invitations.FindByIdAsync(query.InvitationId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<UserInvitation>> Handle(
        GetInvitationsByCompanyIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _invitations.ListByCompanyIdAsync(query.CompanyId, query.Status, cancellationToken);
    }
}
