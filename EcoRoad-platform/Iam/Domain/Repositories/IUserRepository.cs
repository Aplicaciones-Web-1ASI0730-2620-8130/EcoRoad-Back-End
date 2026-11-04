using EcoRoad.Iam.Domain.Model.Aggregates;
using EcoRoad.Shared.Domain.Repositories;

namespace EcoRoad.Iam.Domain.Repositories;

/// <summary>
/// Repository of the <see cref="User" /> aggregate.
/// </summary>
public interface IUserRepository : IBaseRepository<User>
{
    /// <summary>
    /// Finds a user of a company by its email address.
    /// </summary>
    /// <param name="companyId">Identifier of the company.</param>
    /// <param name="email">Normalized email address.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The user when it exists; otherwise <c>null</c>.</returns>
    Task<User?> FindByCompanyIdAndEmailAsync(string companyId, string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists every user registered with an email address, across all companies.
    /// </summary>
    /// <param name="email">Normalized email address.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The users registered with the email.</returns>
    Task<IReadOnlyList<User>> ListByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists the users of a company ordered by name.
    /// </summary>
    /// <param name="companyId">Identifier of the company.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The users of the company.</returns>
    Task<IReadOnlyList<User>> ListByCompanyIdAsync(string companyId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether a company already has any registered user.
    /// </summary>
    /// <param name="companyId">Identifier of the company.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns><c>true</c> when the company has at least one user.</returns>
    Task<bool> ExistsByCompanyIdAsync(string companyId, CancellationToken cancellationToken = default);
}
