using EcoRoad.Alerting.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Alerting.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Registers the Alerting bounded context mappings on the model builder.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the deviation assessment and alert entity configurations.
    /// </summary>
    public static void ApplyAlertingConfiguration(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new DeviationAssessmentConfiguration());
        builder.ApplyConfiguration(new AlertConfiguration());
    }
}
