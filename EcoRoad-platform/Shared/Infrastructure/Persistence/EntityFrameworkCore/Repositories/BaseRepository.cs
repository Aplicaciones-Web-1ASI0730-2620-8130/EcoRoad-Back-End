using EcoRoad.Shared.Domain.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
/// Entity Framework Core implementation of <see cref="IBaseRepository{TEntity}" />.
/// </summary>
/// <remarks>
/// The repository never calls <c>SaveChanges</c>; persistence is coordinated by
/// <see cref="UnitOfWork" /> so a command can touch several aggregates atomically.
/// </remarks>
/// <typeparam name="TEntity">The aggregate or entity type managed by the repository.</typeparam>
public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseRepository{TEntity}" /> class.
    /// </summary>
    /// <param name="context">The shared application database context.</param>
    protected BaseRepository(AppDbContext context)
    {
        Context = context;
    }

    /// <summary>
    /// Gets the shared application database context.
    /// </summary>
    protected AppDbContext Context { get; }

    /// <inheritdoc />
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task<TEntity?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().FindAsync(new object?[] { id }, cancellationToken);
    }

    /// <inheritdoc />
    public void Update(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
    }

    /// <inheritdoc />
    public void Remove(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    /// <inheritdoc />
    public virtual async Task<IReadOnlyList<TEntity>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().ToListAsync(cancellationToken);
    }
}
