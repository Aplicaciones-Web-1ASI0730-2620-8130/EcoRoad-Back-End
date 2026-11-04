using EcoRoad.Iam.Domain.Model.Aggregates;
using EcoRoad.Iam.Domain.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
/// Entity Framework Core implementation of <see cref="IUserRepository" />.
/// </summary>
public class UserRepository : BaseRepository<User>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository" /> class.
    /// </summary>
    /// <param name="context">The shared application database context.</param>
    public UserRepository(AppDbContext context)
        : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<User?> FindByCompanyIdAndEmailAsync(
        string companyId,
        string email,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<User>()
            .FirstOrDefaultAsync(user => user.CompanyId == companyId && user.Email == email, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<User>> ListByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await Context.Set<User>()
            .Where(user => user.Email == email)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<User>> ListByCompanyIdAsync(
        string companyId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<User>()
            .AsNoTracking()
            .Where(user => user.CompanyId == companyId)
            .OrderBy(user => user.Name)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByCompanyIdAsync(string companyId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<User>().AnyAsync(user => user.CompanyId == companyId, cancellationToken);
    }
}
