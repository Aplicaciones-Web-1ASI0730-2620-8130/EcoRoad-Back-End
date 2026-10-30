using EcoRoad.Alerting.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Alerting.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyAlertingConfiguration(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new AlertRuleConfiguration());
        builder.ApplyConfiguration(new AlertConfiguration());
    }
}
