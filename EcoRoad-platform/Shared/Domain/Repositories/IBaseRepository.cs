namespace EcoRoad.Shared.Domain.Repositories;

/// <summary>
/// Defines the basic persistence operations shared by every aggregate repository.
/// </summary>
/// <remarks>
/// Implementations only stage changes in the persistence context. Changes are written
/// to the database when <see cref="IUnitOfWork.CompleteAsync" /> is invoked, which keeps
/// each command handled inside a single transaction.
/// </remarks>
/// <typeparam name="TEntity">The aggregate or entity type managed by the repository.</typeparam>
public interface IBaseRepository<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Stages a new entity to be inserted.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The entity when it exists; otherwise <c>null</c>.</returns>
    Task<TEntity?> FindByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks an entity as modified.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(TEntity entity);

    /// <summary>
    /// Stages an entity to be deleted.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    void Remove(TEntity entity);

    /// <summary>
    /// Lists every entity of the managed type.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A read-only list with all the stored entities.</returns>
    Task<IReadOnlyList<TEntity>> ListAsync(CancellationToken cancellationToken = default);
}
