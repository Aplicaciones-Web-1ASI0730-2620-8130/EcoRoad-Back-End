using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
/// Registers the Entity Framework Core mappings of the Environmental Monitoring bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Applies every monitoring entity configuration to the model.
    /// </summary>
    /// <param name="builder">The model builder of the application context.</param>
    public static void ApplyMonitoringConfiguration(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ThresholdProfileConfiguration());
        builder.ApplyConfiguration(new EnvironmentalMeasurementConfiguration());
    }
}
