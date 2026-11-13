using EcoRoad.Projects.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Projects.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyProjectsConfiguration(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new RoadProjectConfiguration());
        builder.ApplyConfiguration(new RoadSectionConfiguration());
        builder.ApplyConfiguration(new WorkFrontConfiguration());
    }
}
