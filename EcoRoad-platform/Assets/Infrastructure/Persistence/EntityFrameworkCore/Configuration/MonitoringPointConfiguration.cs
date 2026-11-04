using EcoRoad.Assets.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoRoad.Assets.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
/// Maps the <see cref="MonitoringPoint" /> aggregate to the <c>monitoring_points</c> table.
/// </summary>
public class MonitoringPointConfiguration : IEntityTypeConfiguration<MonitoringPoint>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<MonitoringPoint> builder)
    {
        builder.ToTable("monitoring_points");

        builder.HasKey(point => point.Id);

        builder.Property(point => point.Id)
            .HasColumnName("id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(point => point.Code)
            .HasColumnName("code")
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(point => point.ProjectId)
            .HasColumnName("project_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(point => point.SectionId)
            .HasColumnName("section_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(point => point.Name)
            .HasColumnName("name")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(point => point.KilometerPost)
            .HasColumnName("kilometer_post")
            .HasColumnType("varchar(10)")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(point => point.Latitude)
            .HasColumnName("latitude")
            .HasColumnType("double")
            .IsRequired();

        builder.Property(point => point.Longitude)
            .HasColumnName("longitude")
            .HasColumnType("double")
            .IsRequired();

        builder.Property(point => point.Status)
            .HasColumnName("status")
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(point => point.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime");

        builder.Property(point => point.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime");

        builder.HasIndex(point => point.Code)
            .IsUnique();

        builder.HasIndex(point => point.ProjectId);

        builder.HasIndex(point => new { point.SectionId, point.Name })
            .IsUnique();
    }
}
