using EcoRoad.Projects.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Projects.Infrastructure.Data;

public class ProjectDbContext : DbContext
{
    public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
        : base(options)
    {
    }

    public DbSet<RoadProject> RoadProjects => Set<RoadProject>();

    public DbSet<RoadSection> RoadSections => Set<RoadSection>();

    public DbSet<WorkFront> WorkFronts => Set<WorkFront>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectDbContext).Assembly);
    }
}
