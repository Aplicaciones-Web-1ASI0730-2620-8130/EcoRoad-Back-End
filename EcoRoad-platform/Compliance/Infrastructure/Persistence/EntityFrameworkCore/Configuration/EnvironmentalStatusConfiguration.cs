using EcoRoad.Compliance.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoRoad.Compliance.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class EnvironmentalStatusConfiguration : IEntityTypeConfiguration<EnvironmentalStatus>
{
    public void Configure(EntityTypeBuilder<EnvironmentalStatus> builder)
    {
        builder.ToTable("environmental_statuses");

        builder.HasKey(status => status.Id);

        builder.Property(status => status.Id)
            .HasColumnName("id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(status => status.Scope)
            .HasColumnName("scope")
            .HasConversion<string>()
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(status => status.SubjectId)
            .HasColumnName("subject_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(status => status.Standing)
            .HasColumnName("standing")
            .HasConversion<string>()
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(status => status.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime")
            .IsRequired();

        builder.HasIndex(status => new { status.Scope, status.SubjectId })
            .IsUnique();
    }
}
