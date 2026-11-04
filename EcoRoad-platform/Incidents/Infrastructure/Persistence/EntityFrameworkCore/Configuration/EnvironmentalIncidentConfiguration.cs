using EcoRoad.Incidents.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoRoad.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
/// Maps the <see cref="EnvironmentalIncident" /> aggregate to the <c>environmental_incidents</c> table.
/// </summary>
public class EnvironmentalIncidentConfiguration : IEntityTypeConfiguration<EnvironmentalIncident>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<EnvironmentalIncident> builder)
    {
        builder.ToTable("environmental_incidents");

        builder.HasKey(incident => incident.Id);

        builder.Property(incident => incident.Id)
            .HasColumnName("id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(incident => incident.Code)
            .HasColumnName("code")
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(incident => incident.CompanyId)
            .HasColumnName("company_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(incident => incident.AlertId)
            .HasColumnName("alert_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(incident => incident.ProjectId)
            .HasColumnName("project_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(incident => incident.SectionId)
            .HasColumnName("section_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(incident => incident.Title)
            .HasColumnName("title")
            .HasColumnType("varchar(120)")
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(incident => incident.Description)
            .HasColumnName("description")
            .HasColumnType("varchar(1000)")
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(incident => incident.Severity)
            .HasColumnName("severity")
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(incident => incident.Status)
            .HasColumnName("status")
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(incident => incident.OpenedBy)
            .HasColumnName("opened_by")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(incident => incident.OpenedAt)
            .HasColumnName("opened_at")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(incident => incident.ResponsibleUserId)
            .HasColumnName("responsible_user_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36);

        builder.Property(incident => incident.ResponsibleName)
            .HasColumnName("responsible_name")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100);

        builder.Property(incident => incident.AssignedAt)
            .HasColumnName("assigned_at")
            .HasColumnType("datetime");

        builder.Property(incident => incident.AttendedAt)
            .HasColumnName("attended_at")
            .HasColumnType("datetime");

        builder.Property(incident => incident.ClosedBy)
            .HasColumnName("closed_by")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36);

        builder.Property(incident => incident.ClosureNotes)
            .HasColumnName("closure_notes")
            .HasColumnType("varchar(1000)")
            .HasMaxLength(1000);

        builder.Property(incident => incident.ClosedAt)
            .HasColumnName("closed_at")
            .HasColumnType("datetime");

        builder.Property(incident => incident.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime");

        builder.Property(incident => incident.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime");

        builder.HasIndex(incident => incident.AlertId)
            .IsUnique();

        builder.HasIndex(incident => incident.Code)
            .IsUnique();

        builder.HasIndex(incident => new { incident.CompanyId, incident.Status });

        // Corrective actions and evidence belong to the aggregate and are written through its backing fields.
        builder.HasMany(incident => incident.CorrectiveActions)
            .WithOne()
            .HasForeignKey(action => action.IncidentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(incident => incident.FieldEvidences)
            .WithOne()
            .HasForeignKey(evidence => evidence.IncidentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(incident => incident.CorrectiveActions)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(incident => incident.FieldEvidences)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
