using EcoRoad.Iam.Domain.Model.Aggregates;
using EcoRoad.Iam.Domain.Model.ValueObjects;
using EcoRoad.Iam.Domain.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
/// Entity Framework Core implementation of <see cref="IUserInvitationRepository" />.
/// </summary>
public class UserInvitationRepository : BaseRepository<UserInvitation>, IUserInvitationRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserInvitationRepository" /> class.
    /// </summary>
    /// <param name="context">The shared application database context.</param>
    public UserInvitationRepository(AppDbContext context)
        : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<UserInvitation?> FindPendingByCompanyIdAndEmailAsync(
        string companyId,
        string email,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<UserInvitation>()
            .FirstOrDefaultAsync(
                invitation => invitation.CompanyId == companyId
                    && invitation.Email == email
                    && invitation.Status == InvitationStatus.Pending,
                cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<UserInvitation>> ListByCompanyIdAsync(
        string companyId,
        InvitationStatus? status,
        CancellationToken cancellationToken = default)
    {
        var query = Context.Set<UserInvitation>()
            .AsNoTracking()
            .Where(invitation => invitation.CompanyId == companyId);

        if (status.HasValue)
        {
            query = query.Where(invitation => invitation.Status == status.Value);
        }

        return await query
            .OrderByDescending(invitation => invitation.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
