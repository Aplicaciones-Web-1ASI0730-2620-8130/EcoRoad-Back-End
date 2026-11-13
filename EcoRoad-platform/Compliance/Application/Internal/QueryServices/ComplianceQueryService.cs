using EcoRoad.Compliance.Application.QueryServices;
using EcoRoad.Compliance.Domain.Model.Queries;
using EcoRoad.Compliance.Domain.Repositories;

namespace EcoRoad.Compliance.Application.Internal.QueryServices;

public class ComplianceQueryService : IComplianceQueryService
{
    private readonly IComplianceReportRepository _reports;

    public ComplianceQueryService(IComplianceReportRepository reports)
    {
        _reports = reports;
    }

    public async Task<ComplianceReportView?> Handle(
        GetComplianceReportViewQuery query,
        CancellationToken cancellationToken = default)
    {
        var report = await _reports.GetByIdAsync(query.ReportId, cancellationToken);
        if (report == null)
        {
            return null;
        }

        return new ComplianceReportView(
            report.Id,
            report.Scope,
            report.SubjectId,
            report.RequestedBy,
            report.Status,
            report.RequestedAt,
            report.GeneratedAt,
            report.ContentHash,
            report.DigitalSignature,
            report.MonitoringRecordIds,
            report.AlertIds,
            report.IncidentIds,
            report.FieldEvidenceIds,
            report.IsSealed);
    }
}
