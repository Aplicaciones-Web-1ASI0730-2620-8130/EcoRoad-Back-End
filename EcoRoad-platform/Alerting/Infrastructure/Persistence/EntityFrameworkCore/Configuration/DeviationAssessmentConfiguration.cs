using EcoRoad.Alerting.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoRoad.Alerting.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
///     Maps the DeviationAssessment aggregate to deviation_assessments.
/// </summary>
public class DeviationAssessmentConfiguration : IEntityTypeConfiguration<DeviationAssessment>
{
    public void Configure(EntityTypeBuilder<DeviationAssessment> builder)
    {
        builder.ToTable("deviation_assessments");

        builder.HasKey(assessment => assessment.Id);

        builder.Property(assessment => assessment.Id)
            .HasColumnName("id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(assessment => assessment.MeasurementId)
            .HasColumnName("measurement_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(assessment => assessment.MeasuredValue)
            .HasColumnName("measured_value")
            .HasColumnType("double")
            .IsRequired();

        builder.Property(assessment => assessment.ThresholdValue)
            .HasColumnName("threshold_value")
            .HasColumnType("double")
            .IsRequired();

        builder.Property(assessment => assessment.IsCritical)
            .HasColumnName("is_critical")
            .HasColumnType("tinyint(1)")
            .IsRequired();

        builder.Property(assessment => assessment.AssessedAt)
            .HasColumnName("assessed_at")
            .HasColumnType("datetime")
            .IsRequired();

        builder.HasIndex(assessment => assessment.MeasurementId);
    }
}
