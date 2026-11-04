using EcoRoad.Shared.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;

/// <summary>
/// Entity Framework Core interceptor that fills the audit timestamps of every
/// entity implementing <see cref="IAuditableEntity" />.
/// </summary>
public sealed class AuditableEntityInterceptor : SaveChangesInterceptor
{
    /// <inheritdoc />
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ApplyAuditTimestamps(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    /// <inheritdoc />
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplyAuditTimestamps(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// Sets <c>CreatedAt</c> on inserted entities and refreshes <c>UpdatedAt</c> on inserted or modified ones.
    /// </summary>
    /// <param name="context">The context whose tracked entities are inspected.</param>
    private static void ApplyAuditTimestamps(DbContext? context)
    {
        if (context == null)
        {
            return;
        }

        var now = DateTime.UtcNow;
        foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt ??= now;
            }

            if (entry.State is EntityState.Added or EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }
        }
    }
}
