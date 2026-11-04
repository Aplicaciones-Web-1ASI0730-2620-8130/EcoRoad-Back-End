using EcoRoad.Compliance.Domain.Model;
using EcoRoad.Compliance.Domain.Model.ValueObjects;

namespace EcoRoad.Compliance.Domain.Model.Aggregates;

public class ComplianceReport
{
    public ComplianceReport()
    {
        MonitoringRecordIds = new List<string>();
        AlertIds = new List<string>();
        IncidentIds = new List<string>();
        FieldEvidenceIds = new List<string>();
    }

    public ComplianceReport(ComplianceScope scope, string subjectId, string requestedBy)
    {
        Id = Guid.NewGuid().ToString();
        Scope = scope;
        SubjectId = ComplianceRules.RequireId(subjectId, "Subject id");
        RequestedBy = ComplianceRules.Require(requestedBy, "Requested by", 100);
        Status = ComplianceReportStatus.Requested;
        RequestedAt = DateTime.UtcNow;
        MonitoringRecordIds = new List<string>();
        AlertIds = new List<string>();
        IncidentIds = new List<string>();
        FieldEvidenceIds = new List<string>();
    }

    public string Id { get; set; } = string.Empty;

    public ComplianceScope Scope { get; set; }

    public string SubjectId { get; set; } = string.Empty;

    public string RequestedBy { get; set; } = string.Empty;

    public ComplianceReportStatus Status { get; set; }

    public DateTime RequestedAt { get; set; }

    public DateTime? GeneratedAt { get; set; }

    public string? ContentHash { get; set; }

    public string? DigitalSignature { get; set; }

    public List<string> MonitoringRecordIds { get; set; }

    public List<string> AlertIds { get; set; }

    public List<string> IncidentIds { get; set; }

    public List<string> FieldEvidenceIds { get; set; }

    public bool IsSealed => Status == ComplianceReportStatus.Generated;

    public void Generate(ConsolidatedEvidence evidence, string contentHash, string digitalSignature)
    {
        if (IsSealed)
        {
            throw new ComplianceRuleException("A generated compliance report cannot be changed.");
        }

        MonitoringRecordIds = evidence.MonitoringRecordIds.ToList();
        AlertIds = evidence.AlertIds.ToList();
        IncidentIds = evidence.IncidentIds.ToList();
        FieldEvidenceIds = evidence.FieldEvidenceIds.ToList();
        ContentHash = ComplianceRules.RequireSha256(contentHash);
        DigitalSignature = ComplianceRules.Require(digitalSignature, "Digital signature", 2000);
        GeneratedAt = DateTime.UtcNow;
        Status = ComplianceReportStatus.Generated;
    }
}
