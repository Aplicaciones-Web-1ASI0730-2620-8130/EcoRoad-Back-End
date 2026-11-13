using EcoRoad.Compliance.Domain.Model.Queries;

namespace EcoRoad.Compliance.Application.QueryServices;

public interface IComplianceQueryService
{
    Task<ComplianceReportView?> Handle(
        GetComplianceReportViewQuery query,
        CancellationToken cancellationToken = default);
}
