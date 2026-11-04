using EcoRoad.Compliance.Domain.Model.ValueObjects;

namespace EcoRoad.Compliance.Interfaces.Rest.Resources;

public record UpdateEnvironmentalStatusResource(EnvironmentalStanding Standing);

public record RequestComplianceReportResource(ComplianceScope Scope, string SubjectId, string RequestedBy);

public record GenerateComplianceReportResource(
    IReadOnlyList<string>? MonitoringRecordIds,
    IReadOnlyList<string>? AlertIds,
    IReadOnlyList<string>? IncidentIds,
    IReadOnlyList<string>? FieldEvidenceIds,
    string DigitalSignature);

public record EnvironmentalStatusResource(
    string Id,
    ComplianceScope Scope,
    string SubjectId,
    EnvironmentalStanding Standing,
    DateTime UpdatedAt);

public record ComplianceReportResource(
    string Id,
    ComplianceScope Scope,
    string SubjectId,
    string RequestedBy,
    ComplianceReportStatus Status,
    DateTime RequestedAt,
    DateTime? GeneratedAt,
    string? ContentHash,
    string? DigitalSignature,
    IReadOnlyList<string> MonitoringRecordIds,
    IReadOnlyList<string> AlertIds,
    IReadOnlyList<string> IncidentIds,
    IReadOnlyList<string> FieldEvidenceIds,
    bool IsSealed);
