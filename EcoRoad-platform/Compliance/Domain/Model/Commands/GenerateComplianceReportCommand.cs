namespace EcoRoad.Compliance.Domain.Model.Commands;

public record GenerateComplianceReportCommand(
    string ReportId,
    IReadOnlyList<string> MonitoringRecordIds,
    IReadOnlyList<string> AlertIds,
    IReadOnlyList<string> IncidentIds,
    IReadOnlyList<string> FieldEvidenceIds,
    string DigitalSignature);
