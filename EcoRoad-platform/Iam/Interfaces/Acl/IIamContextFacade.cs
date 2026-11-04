namespace EcoRoad.Iam.Interfaces.Acl;

/// <summary>
/// Anti-corruption layer that exposes the IAM capabilities other bounded contexts may need,
/// without leaking the IAM domain model.
/// </summary>
public interface IIamContextFacade
{
    /// <summary>
    /// Checks whether a user is an active collaborator of a company.
    /// </summary>
    /// <param name="companyId">Identifier of the company.</param>
    /// <param name="userId">Identifier of the user.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns><c>true</c> when the user belongs to the company and has active access.</returns>
    Task<bool> IsActiveCollaboratorAsync(string companyId, string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether a user holds a permission.
    /// </summary>
    /// <param name="userId">Identifier of the user.</param>
    /// <param name="permission">Name of the permission, for example <c>ManageIncidents</c>.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns><c>true</c> when the user has active access and the permission.</returns>
    Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the display name of a user.
    /// </summary>
    /// <param name="userId">Identifier of the user.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The name of the user, or an empty string when it does not exist.</returns>
    Task<string> FetchUserNameAsync(string userId, CancellationToken cancellationToken = default);
}
