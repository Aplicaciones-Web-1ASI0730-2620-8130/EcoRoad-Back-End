using EcoRoad.Monitoring.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoRoad.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
/// Maps the <see cref="EnvironmentalMeasurement" /> aggregate to the <c>environmental_measurements</c> table.
/// </summary>
public class EnvironmentalMeasurementConfiguration : IEntityTypeConfiguration<EnvironmentalMeasurement>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<EnvironmentalMeasurement> builder)
    {
        builder.ToTable("environmental_measurements");

        builder.HasKey(measurement => measurement.Id);

        builder.Property(measurement => measurement.Id)
            .HasColumnName("id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(measurement => measurement.ProjectId)
            .HasColumnName("project_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(measurement => measurement.SectionId)
            .HasColumnName("section_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(measurement => measurement.MonitoringPointId)
            .HasColumnName("monitoring_point_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36);

        builder.Property(measurement => measurement.SensorId)
            .HasColumnName("sensor_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36);

        builder.Property(measurement => measurement.RecordedBy)
            .HasColumnName("recorded_by")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36);

        builder.Property(measurement => measurement.ParameterType)
            .HasColumnName("parameter_type")
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(measurement => measurement.Value)
            .HasColumnName("value")
            .HasColumnType("double")
            .IsRequired();

        builder.Property(measurement => measurement.Unit)
            .HasColumnName("unit")
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(measurement => measurement.Source)
            .HasColumnName("source")
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(measurement => measurement.Notes)
            .HasColumnName("notes")
            .HasColumnType("varchar(500)")
            .HasMaxLength(500);

        builder.Property(measurement => measurement.RecordedAt)
            .HasColumnName("recorded_at")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(measurement => measurement.Status)
            .HasColumnName("status")
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(measurement => measurement.ThresholdProfileId)
            .HasColumnName("threshold_profile_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(measurement => measurement.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime");

        builder.Property(measurement => measurement.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime");

        builder.HasIndex(measurement => new { measurement.ProjectId, measurement.ParameterType, measurement.RecordedAt });

        builder.HasIndex(measurement => measurement.SensorId);

        builder.HasOne<ThresholdProfile>()
            .WithMany()
            .HasForeignKey(measurement => measurement.ThresholdProfileId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
