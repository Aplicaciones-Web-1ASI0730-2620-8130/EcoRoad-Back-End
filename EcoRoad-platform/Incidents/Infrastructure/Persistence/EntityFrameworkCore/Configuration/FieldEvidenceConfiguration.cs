using EcoRoad.Incidents.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoRoad.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
/// Maps the <see cref="FieldEvidence" /> entity to the <c>field_evidences</c> table.
/// </summary>
public class FieldEvidenceConfiguration : IEntityTypeConfiguration<FieldEvidence>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<FieldEvidence> builder)
    {
        builder.ToTable("field_evidences");

        builder.HasKey(evidence => evidence.Id);

        builder.Property(evidence => evidence.Id)
            .HasColumnName("id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(evidence => evidence.IncidentId)
            .HasColumnName("incident_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(evidence => evidence.RegisteredByUserId)
            .HasColumnName("registered_by_user_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(evidence => evidence.FileUrl)
            .HasColumnName("file_url")
            .HasColumnType("varchar(500)")
            .HasMaxLength(500);

        builder.Property(evidence => evidence.Notes)
            .HasColumnName("notes")
            .HasColumnType("varchar(1000)")
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(evidence => evidence.RecordedAt)
            .HasColumnName("recorded_at")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(evidence => evidence.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime");

        builder.Property(evidence => evidence.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime");
    }
}
