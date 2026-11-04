using EcoRoad.Iam.Domain.Model;
using EcoRoad.Iam.Domain.Model.Aggregates;
using EcoRoad.Iam.Domain.Model.ValueObjects;
using EcoRoad.Shared.Domain.Model.Exceptions;

namespace EcoRoad.Iam.Domain.Services;

/// <summary>
/// Domain service with the rules an administrator must satisfy to manage other collaborators.
/// </summary>
/// <remarks>
/// These rules involve two users (the acting administrator and the target collaborator),
/// so they live outside the <see cref="User" /> aggregate.
/// </remarks>
public static class AccessManagementPolicy
{
    /// <summary>
    /// Ensures the actor is an active administrator allowed to manage users.
    /// </summary>
    /// <param name="actor">The authenticated user performing the operation.</param>
    public static void EnsureIsAdministrator(User actor)
    {
        if (!actor.HasPermission(Permission.ManageUsers))
        {
            throw new IamRuleException(
                "ADMIN_REQUIRED",
                "Only an administrator with active access can manage collaborators.",
                DomainErrorKind.Forbidden);
        }
    }

    /// <summary>
    /// Ensures the actor can manage the target collaborator.
    /// </summary>
    /// <param name="actor">The authenticated user performing the operation.</param>
    /// <param name="target">The collaborator being managed.</param>
    public static void EnsureCanManage(User actor, User target)
    {
        EnsureIsAdministrator(actor);
        if (actor.CompanyId != target.CompanyId)
        {
            throw new IamRuleException(
                "CROSS_COMPANY_FORBIDDEN",
                "You can only manage collaborators of your own company.",
                DomainErrorKind.Forbidden);
        }
    }

    /// <summary>
    /// Prevents administrators from revoking their own access.
    /// </summary>
    /// <param name="actor">The authenticated user performing the operation.</param>
    /// <param name="target">The collaborator whose access is revoked.</param>
    public static void EnsureIsNotSelfRevocation(User actor, User target)
    {
        if (actor.Id == target.Id)
        {
            throw new IamRuleException(
                "SELF_REVOKE_FORBIDDEN",
                "You cannot revoke your own access.",
                DomainErrorKind.Conflict);
        }
    }

    /// <summary>
    /// Prevents administrators from removing their own administrator role.
    /// </summary>
    /// <param name="actor">The authenticated user performing the operation.</param>
    /// <param name="target">The collaborator whose role changes.</param>
    /// <param name="role">The new role.</param>
    public static void EnsureKeepsOwnAdministratorRole(User actor, User target, Role role)
    {
        if (actor.Id == target.Id && role != Role.CompanyAdmin)
        {
            throw new IamRuleException(
                "SELF_LOCKOUT_FORBIDDEN",
                "You cannot remove your own administrator role.",
                DomainErrorKind.Conflict);
        }
    }

    /// <summary>
    /// Prevents administrators from removing their own permission to manage users.
    /// </summary>
    /// <param name="actor">The authenticated user performing the operation.</param>
    /// <param name="target">The collaborator whose permissions change.</param>
    /// <param name="permissions">The new set of permissions.</param>
    public static void EnsureKeepsOwnUserManagement(User actor, User target, IEnumerable<Permission> permissions)
    {
        if (actor.Id == target.Id && !permissions.Contains(Permission.ManageUsers))
        {
            throw new IamRuleException(
                "SELF_LOCKOUT_FORBIDDEN",
                "Keep your permission to manage collaborators.",
                DomainErrorKind.Conflict);
        }
    }
}
