using EcoRoad.Iam.Domain.Model.Aggregates;
using EcoRoad.Iam.Domain.Model.ValueObjects;
using EcoRoad.Shared.Domain.Repositories;

namespace EcoRoad.Iam.Domain.Repositories;

/// <summary>
/// Repository of the <see cref="UserInvitation" /> aggregate.
/// </summary>
public interface IUserInvitationRepository : IBaseRepository<UserInvitation>
{
    /// <summary>
    /// Finds the pending invitation sent to an email address by a company.
    /// </summary>
    /// <param name="companyId">Identifier of the company.</param>
    /// <param name="email">Normalized email address.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The pending invitation when it exists; otherwise <c>null</c>.</returns>
    Task<UserInvitation?> FindPendingByCompanyIdAndEmailAsync(
        string companyId,
        string email,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists the invitations of a company, newest first.
    /// </summary>
    /// <param name="companyId">Identifier of the company.</param>
    /// <param name="status">Optional status filter.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The invitations of the company.</returns>
    Task<IReadOnlyList<UserInvitation>> ListByCompanyIdAsync(
        string companyId,
        InvitationStatus? status,
        CancellationToken cancellationToken = default);
}
