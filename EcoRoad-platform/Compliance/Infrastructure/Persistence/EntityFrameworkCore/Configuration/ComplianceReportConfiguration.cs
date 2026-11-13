using System.Text.Json;
using EcoRoad.Compliance.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoRoad.Compliance.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class ComplianceReportConfiguration : IEntityTypeConfiguration<ComplianceReport>
{
    public void Configure(EntityTypeBuilder<ComplianceReport> builder)
    {
        builder.ToTable("compliance_reports");

        builder.HasKey(report => report.Id);

        builder.Property(report => report.Id)
            .HasColumnName("id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(report => report.Scope)
            .HasColumnName("scope")
            .HasConversion<string>()
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(report => report.SubjectId)
            .HasColumnName("subject_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(report => report.RequestedBy)
            .HasColumnName("requested_by")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(report => report.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(report => report.RequestedAt)
            .HasColumnName("requested_at")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(report => report.GeneratedAt)
            .HasColumnName("generated_at")
            .HasColumnType("datetime")
            .IsRequired(false);

        builder.Property(report => report.ContentHash)
            .HasColumnName("content_hash")
            .HasColumnType("varchar(64)")
            .HasMaxLength(64)
            .IsRequired(false);

        builder.Property(report => report.DigitalSignature)
            .HasColumnName("digital_signature")
            .HasColumnType("varchar(2000)")
            .HasMaxLength(2000)
            .IsRequired(false);

        MapIdList(builder.Property(report => report.MonitoringRecordIds), "monitoring_record_ids");
        MapIdList(builder.Property(report => report.AlertIds), "alert_ids");
        MapIdList(builder.Property(report => report.IncidentIds), "incident_ids");
        MapIdList(builder.Property(report => report.FieldEvidenceIds), "field_evidence_ids");

        builder.Ignore(report => report.IsSealed);
    }

    private static void MapIdList(PropertyBuilder<List<string>> property, string columnName)
    {
        property
            .HasColumnName(columnName)
            .HasColumnType("json")
            .HasConversion(
                ids => JsonSerializer.Serialize(ids, (JsonSerializerOptions?)null),
                json => JsonSerializer.Deserialize<List<string>>(json, (JsonSerializerOptions?)null) ?? new List<string>())
            .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (left, right) => (left ?? new List<string>()).SequenceEqual(right ?? new List<string>()),
                value => value.Aggregate(0, (hash, item) => HashCode.Combine(hash, item.GetHashCode())),
                value => value.ToList()));
    }
}
