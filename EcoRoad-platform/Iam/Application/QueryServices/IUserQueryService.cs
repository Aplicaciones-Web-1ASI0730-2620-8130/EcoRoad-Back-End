using EcoRoad.Iam.Domain.Model.Aggregates;
using EcoRoad.Iam.Domain.Model.Queries;

namespace EcoRoad.Iam.Application.QueryServices;

/// <summary>
/// Application service that answers the queries about users.
/// </summary>
public interface IUserQueryService
{
    /// <summary>Retrieves a user by its identifier.</summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The user when it exists; otherwise <c>null</c>.</returns>
    Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken = default);

    /// <summary>Lists the collaborators of a company.</summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The collaborators of the company.</returns>
    Task<IReadOnlyList<User>> Handle(GetUsersByCompanyIdQuery query, CancellationToken cancellationToken = default);
}
