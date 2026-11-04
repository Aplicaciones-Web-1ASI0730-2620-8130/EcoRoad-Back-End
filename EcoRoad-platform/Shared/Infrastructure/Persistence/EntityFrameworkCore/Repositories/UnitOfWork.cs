using EcoRoad.Shared.Domain.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
/// Entity Framework Core implementation of <see cref="IUnitOfWork" />.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork" /> class.
    /// </summary>
    /// <param name="context">The shared application database context.</param>
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
