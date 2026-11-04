using EcoRoad.Assets.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoRoad.Assets.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
/// Maps the <see cref="Sensor" /> aggregate to the <c>sensors</c> table.
/// </summary>
public class SensorConfiguration : IEntityTypeConfiguration<Sensor>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Sensor> builder)
    {
        builder.ToTable("sensors");

        builder.HasKey(sensor => sensor.Id);

        builder.Property(sensor => sensor.Id)
            .HasColumnName("id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(sensor => sensor.Code)
            .HasColumnName("code")
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(sensor => sensor.Name)
            .HasColumnName("name")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(sensor => sensor.SerialNumber)
            .HasColumnName("serial_number")
            .HasColumnType("varchar(40)")
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(sensor => sensor.Type)
            .HasColumnName("type")
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(sensor => sensor.Model)
            .HasColumnName("model")
            .HasColumnType("varchar(60)")
            .HasMaxLength(60);

        builder.Property(sensor => sensor.Status)
            .HasColumnName("status")
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(sensor => sensor.MonitoringPointId)
            .HasColumnName("monitoring_point_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36);

        builder.Property(sensor => sensor.ProjectId)
            .HasColumnName("project_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36);

        builder.Property(sensor => sensor.SectionId)
            .HasColumnName("section_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36);

        builder.Property(sensor => sensor.CalibratedAt)
            .HasColumnName("calibrated_at")
            .HasColumnType("datetime");

        builder.Property(sensor => sensor.CalibrationValidUntil)
            .HasColumnName("calibration_valid_until")
            .HasColumnType("datetime");

        builder.Property(sensor => sensor.ActivatedAt)
            .HasColumnName("activated_at")
            .HasColumnType("datetime");

        builder.Property(sensor => sensor.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime");

        builder.Property(sensor => sensor.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime");

        builder.HasIndex(sensor => sensor.Code)
            .IsUnique();

        builder.HasIndex(sensor => sensor.SerialNumber)
            .IsUnique();

        builder.HasIndex(sensor => sensor.ProjectId);

        builder.HasIndex(sensor => sensor.Status);

        // A sensor references its current point by identifier; points in use cannot be deleted.
        builder.HasOne<MonitoringPoint>()
            .WithMany()
            .HasForeignKey(sensor => sensor.MonitoringPointId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
