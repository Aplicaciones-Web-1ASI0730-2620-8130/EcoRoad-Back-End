using EcoRoad.Iam.Domain.Model.Aggregates;
using EcoRoad.Iam.Interfaces.Rest.Resources;

namespace EcoRoad.Iam.Interfaces.Rest.Transform;

/// <summary>
/// Converts <see cref="User" /> aggregates into <see cref="UserResource" /> objects.
/// </summary>
public static class UserResourceFromEntityAssembler
{
    /// <summary>
    /// Converts a user into its resource.
    /// </summary>
    /// <param name="user">The user to convert.</param>
    /// <returns>The user resource.</returns>
    public static UserResource ToResourceFromEntity(User user)
    {
        return new UserResource(
            user.Id,
            user.CompanyId,
            user.Name,
            user.Email,
            user.Role,
            user.Permissions,
            user.AccessStatus,
            user.GrantedAt,
            user.RevokedAt,
            user.CreatedAt);
    }

    /// <summary>
    /// Converts a list of users into resources.
    /// </summary>
    /// <param name="users">The users to convert.</param>
    /// <returns>The user resources.</returns>
    public static IReadOnlyList<UserResource> ToResourcesFromEntities(IEnumerable<User> users)
    {
        return users.Select(ToResourceFromEntity).ToList();
    }
}
