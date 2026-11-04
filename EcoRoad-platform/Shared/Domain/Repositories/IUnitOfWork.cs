namespace EcoRoad.Shared.Domain.Repositories;

/// <summary>
/// Coordinates the persistence of every change staged by the repositories during a command.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Persists all the pending changes in a single database transaction.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    Task CompleteAsync(CancellationToken cancellationToken = default);
}
