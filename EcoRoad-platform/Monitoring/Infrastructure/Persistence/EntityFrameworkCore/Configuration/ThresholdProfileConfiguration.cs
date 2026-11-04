using EcoRoad.Monitoring.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoRoad.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
/// Maps the <see cref="ThresholdProfile" /> aggregate to the <c>threshold_profiles</c> table.
/// </summary>
public class ThresholdProfileConfiguration : IEntityTypeConfiguration<ThresholdProfile>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ThresholdProfile> builder)
    {
        builder.ToTable("threshold_profiles");

        builder.HasKey(profile => profile.Id);

        builder.Property(profile => profile.Id)
            .HasColumnName("id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(profile => profile.Name)
            .HasColumnName("name")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(profile => profile.ParameterType)
            .HasColumnName("parameter_type")
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(profile => profile.Unit)
            .HasColumnName("unit")
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(profile => profile.Kind)
            .HasColumnName("kind")
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(profile => profile.WarningThreshold)
            .HasColumnName("warning_threshold")
            .HasColumnType("double");

        builder.Property(profile => profile.CriticalThreshold)
            .HasColumnName("critical_threshold")
            .HasColumnType("double");

        builder.Property(profile => profile.Minimum)
            .HasColumnName("minimum")
            .HasColumnType("double");

        builder.Property(profile => profile.OptimalMinimum)
            .HasColumnName("optimal_minimum")
            .HasColumnType("double");

        builder.Property(profile => profile.OptimalMaximum)
            .HasColumnName("optimal_maximum")
            .HasColumnType("double");

        builder.Property(profile => profile.Maximum)
            .HasColumnName("maximum")
            .HasColumnType("double");

        builder.Property(profile => profile.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime");

        builder.Property(profile => profile.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime");

        builder.HasIndex(profile => profile.ParameterType)
            .IsUnique();
    }
}
