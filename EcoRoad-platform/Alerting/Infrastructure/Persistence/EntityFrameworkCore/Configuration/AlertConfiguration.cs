using EcoRoad.Alerting.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoRoad.Alerting.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class AlertConfiguration : IEntityTypeConfiguration<Alert>
{
    public void Configure(EntityTypeBuilder<Alert> builder)
    {
        builder.ToTable("alerts");

        builder.HasKey(alert => alert.Id);

        builder.Property(alert => alert.Id)
            .HasColumnName("id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(alert => alert.WorkFrontId)
            .HasColumnName("work_front_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(alert => alert.AlertRuleId)
            .HasColumnName("alert_rule_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(alert => alert.Parameter)
            .HasColumnName("parameter")
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(alert => alert.Unit)
            .HasColumnName("unit")
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(alert => alert.TimeBand)
            .HasColumnName("time_band")
            .HasConversion<string>()
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(alert => alert.MeasuredValue)
            .HasColumnName("measured_value")
            .HasColumnType("double")
            .IsRequired();

        builder.Property(alert => alert.LimitValue)
            .HasColumnName("limit_value")
            .HasColumnType("double")
            .IsRequired();

        builder.Property(alert => alert.Severity)
            .HasColumnName("severity")
            .HasConversion<string>()
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(alert => alert.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(alert => alert.RaisedAt)
            .HasColumnName("raised_at")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(alert => alert.AcknowledgedAt)
            .HasColumnName("acknowledged_at")
            .HasColumnType("datetime")
            .IsRequired(false);

        builder.Property(alert => alert.AcknowledgedBy)
            .HasColumnName("acknowledged_by")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Ignore(alert => alert.IsActive);
        builder.Ignore(alert => alert.RequiresImmediateNotification);

        builder.HasOne<AlertRule>()
            .WithMany()
            .HasForeignKey(alert => alert.AlertRuleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(alert => alert.WorkFrontId);
        builder.HasIndex(alert => alert.Status);
    }
}
