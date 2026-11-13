using EcoRoad.Compliance.Domain.Model.Aggregates;
using EcoRoad.Compliance.Domain.Model.Commands;

namespace EcoRoad.Compliance.Application.CommandServices;

public interface IComplianceCommandService
{
    Task<EnvironmentalStatus> Handle(
        UpdateProjectEnvironmentalStatusCommand command,
        CancellationToken cancellationToken = default);

    Task<EnvironmentalStatus> Handle(
        UpdatePortfolioEnvironmentalStatusCommand command,
        CancellationToken cancellationToken = default);

    Task<ComplianceReport> Handle(
        RequestComplianceReportCommand command,
        CancellationToken cancellationToken = default);

    Task<ComplianceReport> Handle(
        GenerateComplianceReportCommand command,
        CancellationToken cancellationToken = default);
}
