using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
/// Registers the Entity Framework Core mappings of the IAM bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Applies every IAM entity configuration to the model.
    /// </summary>
    /// <param name="builder">The model builder of the application context.</param>
    public static void ApplyIamConfiguration(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new UserInvitationConfiguration());
    }
}
