using EcoRoad.Iam.Application.QueryServices;
using EcoRoad.Iam.Domain.Model.Queries;
using EcoRoad.Iam.Domain.Model.ValueObjects;
using EcoRoad.Iam.Interfaces.Acl;

namespace EcoRoad.Iam.Application.Acl;

/// <summary>
/// Default implementation of <see cref="IIamContextFacade" /> backed by the IAM query services.
/// </summary>
public class IamContextFacade : IIamContextFacade
{
    private readonly IUserQueryService _userQueries;

    /// <summary>
    /// Initializes a new instance of the <see cref="IamContextFacade" /> class.
    /// </summary>
    /// <param name="userQueries">Query service of users.</param>
    public IamContextFacade(IUserQueryService userQueries)
    {
        _userQueries = userQueries;
    }

    /// <inheritdoc />
    public async Task<bool> IsActiveCollaboratorAsync(
        string companyId,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userQueries.Handle(new GetUserByIdQuery(userId), cancellationToken);
        return user != null && user.CompanyId == companyId && user.CanSignIn;
    }

    /// <inheritdoc />
    public async Task<bool> HasPermissionAsync(
        string userId,
        string permission,
        CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<Permission>(permission, ignoreCase: true, out var parsed))
        {
            return false;
        }

        var user = await _userQueries.Handle(new GetUserByIdQuery(userId), cancellationToken);
        return user != null && user.HasPermission(parsed);
    }

    /// <inheritdoc />
    public async Task<string> FetchUserNameAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userQueries.Handle(new GetUserByIdQuery(userId), cancellationToken);
        return user?.Name ?? string.Empty;
    }
}
