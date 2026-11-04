using EcoRoad.Iam.Application.QueryServices;
using EcoRoad.Iam.Domain.Model.Aggregates;
using EcoRoad.Iam.Domain.Model.Queries;
using EcoRoad.Iam.Domain.Repositories;

namespace EcoRoad.Iam.Application.Internal.QueryServices;

/// <summary>
/// Answers the queries about users.
/// </summary>
public class UserQueryService : IUserQueryService
{
    private readonly IUserRepository _users;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserQueryService" /> class.
    /// </summary>
    /// <param name="users">Repository of users.</param>
    public UserQueryService(IUserRepository users)
    {
        _users = users;
    }

    /// <inheritdoc />
    public async Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _users.FindByIdAsync(query.UserId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<User>> Handle(GetUsersByCompanyIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _users.ListByCompanyIdAsync(query.CompanyId, cancellationToken);
    }
}
