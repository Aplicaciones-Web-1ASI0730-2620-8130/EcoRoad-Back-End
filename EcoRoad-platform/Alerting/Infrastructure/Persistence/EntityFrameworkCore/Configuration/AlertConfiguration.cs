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

        builder.Property(alert => alert.AssessmentId)
            .HasColumnName("assessment_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(alert => alert.MonitoringPointId)
            .HasColumnName("monitoring_point_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
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

        builder.Property(alert => alert.Message)
            .HasColumnName("message")
            .HasColumnType("varchar(500)")
            .HasMaxLength(500)
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

        builder.HasOne<DeviationAssessment>()
            .WithOne()
            .HasForeignKey<Alert>(alert => alert.AssessmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(alert => alert.MonitoringPointId);
        builder.HasIndex(alert => alert.Status);
    }
}
