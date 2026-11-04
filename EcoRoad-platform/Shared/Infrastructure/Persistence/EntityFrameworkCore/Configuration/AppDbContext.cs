using EcoRoad.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
/// Database context shared by every bounded context of the modular monolith.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext" /> class.
    /// </summary>
    /// <param name="options">Options configured at startup.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(optionsBuilder);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyMonitoringConfiguration();
    }
}
