using EcoRoad.Compliance.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Compliance.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyComplianceConfiguration(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new EnvironmentalStatusConfiguration());
        builder.ApplyConfiguration(new ComplianceReportConfiguration());
    }
}
