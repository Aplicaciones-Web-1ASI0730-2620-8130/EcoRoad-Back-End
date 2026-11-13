using EcoRoad.Alerting.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoRoad.Alerting.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class AlertRuleConfiguration : IEntityTypeConfiguration<AlertRule>
{
    public void Configure(EntityTypeBuilder<AlertRule> builder)
    {
        builder.ToTable("alert_rules");

        builder.HasKey(rule => rule.Id);

        builder.Property(rule => rule.Id)
            .HasColumnName("id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(rule => rule.Parameter)
            .HasColumnName("parameter")
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(rule => rule.Unit)
            .HasColumnName("unit")
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(rule => rule.TimeBand)
            .HasColumnName("time_band")
            .HasConversion<string>()
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(rule => rule.Limit)
            .HasColumnName("limit")
            .HasColumnType("double")
            .IsRequired();

        builder.Property(rule => rule.CriticalExcess)
            .HasColumnName("critical_excess")
            .HasColumnType("double")
            .IsRequired();

        builder.HasIndex(rule => new { rule.Parameter, rule.TimeBand })
            .IsUnique();
    }
}
