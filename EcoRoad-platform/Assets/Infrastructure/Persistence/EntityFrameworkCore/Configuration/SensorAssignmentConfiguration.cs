using EcoRoad.Assets.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoRoad.Assets.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
/// Maps the <see cref="SensorAssignment" /> history to the <c>sensor_assignments</c> table.
/// </summary>
public class SensorAssignmentConfiguration : IEntityTypeConfiguration<SensorAssignment>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<SensorAssignment> builder)
    {
        builder.ToTable("sensor_assignments");

        builder.HasKey(assignment => assignment.Id);

        builder.Property(assignment => assignment.Id)
            .HasColumnName("id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(assignment => assignment.SensorId)
            .HasColumnName("sensor_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(assignment => assignment.MonitoringPointId)
            .HasColumnName("monitoring_point_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(assignment => assignment.AssignedAt)
            .HasColumnName("assigned_at")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(assignment => assignment.ReleasedAt)
            .HasColumnName("released_at")
            .HasColumnType("datetime");

        builder.Property(assignment => assignment.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime");

        builder.Property(assignment => assignment.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime");

        builder.HasOne<Sensor>()
            .WithMany()
            .HasForeignKey(assignment => assignment.SensorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<MonitoringPoint>()
            .WithMany()
            .HasForeignKey(assignment => assignment.MonitoringPointId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
