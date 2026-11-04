using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Assets.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
/// Registers the Entity Framework Core mappings of the Monitoring Asset and Deployment bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Applies every asset entity configuration to the model.
    /// </summary>
    /// <param name="builder">The model builder of the application context.</param>
    public static void ApplyAssetsConfiguration(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new MonitoringPointConfiguration());
        builder.ApplyConfiguration(new SensorConfiguration());
        builder.ApplyConfiguration(new SensorAssignmentConfiguration());
    }
}
