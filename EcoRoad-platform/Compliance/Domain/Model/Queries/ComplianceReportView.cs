using EcoRoad.Compliance.Domain.Model.ValueObjects;

namespace EcoRoad.Compliance.Domain.Model.Queries;

public record ComplianceReportView(
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
