using EcoRoad.Iam.Domain.Model.Aggregates;
using EcoRoad.Iam.Domain.Model.Queries;

namespace EcoRoad.Iam.Application.QueryServices;

/// <summary>
/// Application service that answers the queries about invitations.
/// </summary>
public interface IUserInvitationQueryService
{
    /// <summary>Retrieves an invitation by its identifier.</summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The invitation when it exists; otherwise <c>null</c>.</returns>
    Task<UserInvitation?> Handle(GetInvitationByIdQuery query, CancellationToken cancellationToken = default);

    /// <summary>Lists the invitations of a company.</summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The invitations of the company.</returns>
    Task<IReadOnlyList<UserInvitation>> Handle(GetInvitationsByCompanyIdQuery query, CancellationToken cancellationToken = default);
}
