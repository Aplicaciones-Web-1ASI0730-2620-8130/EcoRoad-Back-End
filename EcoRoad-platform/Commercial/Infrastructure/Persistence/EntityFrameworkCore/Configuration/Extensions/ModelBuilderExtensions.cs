using EcoRoad.Commercial.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Commercial.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyCommercialConfiguration(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CompanyAccountConfiguration());
        builder.ApplyConfiguration(new SubscriptionConfiguration());
    }
}
