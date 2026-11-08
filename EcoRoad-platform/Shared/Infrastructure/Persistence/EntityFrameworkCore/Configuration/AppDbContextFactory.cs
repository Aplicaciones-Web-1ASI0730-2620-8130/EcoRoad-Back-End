using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
/// Creates the context for EF tooling without starting the web application or connecting to the database.
/// </summary>
public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySQL("server=localhost;database=ecoroad;user=design_time;password=design_time")
            .Options;

        return new AppDbContext(options);
    }
}
