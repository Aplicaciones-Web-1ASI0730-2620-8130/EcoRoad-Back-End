using EcoRoad.Compliance.Domain.Model;

namespace EcoRoad.Compliance.Domain.Model.ValueObjects;

public class ConsolidatedEvidence
{
    public ConsolidatedEvidence(
        IEnumerable<string>? monitoringRecordIds,
        IEnumerable<string>? alertIds,
        IEnumerable<string>? incidentIds,
        IEnumerable<string>? fieldEvidenceIds)
    {
        MonitoringRecordIds = ComplianceRules.RequireIds(monitoringRecordIds, "Monitoring record id");
        AlertIds = ComplianceRules.RequireIds(alertIds, "Alert id");
        IncidentIds = ComplianceRules.RequireIds(incidentIds, "Incident id");
        FieldEvidenceIds = ComplianceRules.RequireIds(fieldEvidenceIds, "Field evidence id");
    }

    public IReadOnlyList<string> MonitoringRecordIds { get; }

    public IReadOnlyList<string> AlertIds { get; }

    public IReadOnlyList<string> IncidentIds { get; }

    public IReadOnlyList<string> FieldEvidenceIds { get; }
}
