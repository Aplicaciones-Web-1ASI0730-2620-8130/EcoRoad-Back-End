using EcoRoad.Compliance.Domain.Model.ValueObjects;

namespace EcoRoad.Compliance.Domain.Model.Commands;

public record RequestComplianceReportCommand(ComplianceScope Scope, string SubjectId, string RequestedBy);
