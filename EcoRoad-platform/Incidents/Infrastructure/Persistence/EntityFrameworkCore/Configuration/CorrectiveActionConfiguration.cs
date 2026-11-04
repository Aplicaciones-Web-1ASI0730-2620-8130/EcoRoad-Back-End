using EcoRoad.Incidents.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoRoad.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
/// Maps the <see cref="CorrectiveAction" /> entity to the <c>corrective_actions</c> table.
/// </summary>
public class CorrectiveActionConfiguration : IEntityTypeConfiguration<CorrectiveAction>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<CorrectiveAction> builder)
    {
        builder.ToTable("corrective_actions");

        builder.HasKey(action => action.Id);

        builder.Property(action => action.Id)
            .HasColumnName("id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(action => action.IncidentId)
            .HasColumnName("incident_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(action => action.Description)
            .HasColumnName("description")
            .HasColumnType("varchar(500)")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(action => action.Status)
            .HasColumnName("status")
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(action => action.RemediationProgress)
            .HasColumnName("remediation_progress")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(action => action.AssignedBy)
            .HasColumnName("assigned_by")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(action => action.AssignedAt)
            .HasColumnName("assigned_at")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(action => action.StartedAt)
            .HasColumnName("started_at")
            .HasColumnType("datetime");

        builder.Property(action => action.ExecutedAt)
            .HasColumnName("executed_at")
            .HasColumnType("datetime");

        builder.Property(action => action.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime");

        builder.Property(action => action.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime");
    }
}
