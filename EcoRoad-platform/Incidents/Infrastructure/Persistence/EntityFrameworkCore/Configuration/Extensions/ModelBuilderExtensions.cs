using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
/// Registers the Entity Framework Core mappings of the Incident and Remediation Management bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Applies every incident entity configuration to the model.
    /// </summary>
    /// <param name="builder">The model builder of the application context.</param>
    public static void ApplyIncidentsConfiguration(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new EnvironmentalIncidentConfiguration());
        builder.ApplyConfiguration(new CorrectiveActionConfiguration());
        builder.ApplyConfiguration(new FieldEvidenceConfiguration());
    }
}
