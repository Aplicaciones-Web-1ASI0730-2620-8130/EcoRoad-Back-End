using System.Security.Cryptography;
using System.Text;
using EcoRoad.Compliance.Application.CommandServices;
using EcoRoad.Compliance.Domain.Model;
using EcoRoad.Compliance.Domain.Model.Aggregates;
using EcoRoad.Compliance.Domain.Model.Commands;
using EcoRoad.Compliance.Domain.Model.ValueObjects;
using EcoRoad.Compliance.Domain.Repositories;

namespace EcoRoad.Compliance.Application.Internal.CommandServices;

public class ComplianceCommandService : IComplianceCommandService
{
    private readonly IEnvironmentalStatusRepository _statuses;
    private readonly IComplianceReportRepository _reports;

    public ComplianceCommandService(
        IEnvironmentalStatusRepository statuses,
        IComplianceReportRepository reports)
    {
        _statuses = statuses;
        _reports = reports;
    }

    public Task<EnvironmentalStatus> Handle(
        UpdateProjectEnvironmentalStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        return UpdateStatusAsync(ComplianceScope.Project, command.ProjectId, command.Standing, cancellationToken);
    }

    public Task<EnvironmentalStatus> Handle(
        UpdatePortfolioEnvironmentalStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        return UpdateStatusAsync(ComplianceScope.Portfolio, command.PortfolioId, command.Standing, cancellationToken);
    }

    public async Task<ComplianceReport> Handle(
        RequestComplianceReportCommand command,
        CancellationToken cancellationToken = default)
    {
        var report = new ComplianceReport(command.Scope, command.SubjectId, command.RequestedBy);
        await _reports.AddAsync(report, cancellationToken);
        return report;
    }

    public async Task<ComplianceReport> Handle(
        GenerateComplianceReportCommand command,
        CancellationToken cancellationToken = default)
    {
        var report = await _reports.GetByIdAsync(command.ReportId, cancellationToken);
        if (report == null)
        {
            throw new ComplianceRuleException("Compliance report was not found.", notFound: true);
        }

        var evidence = new ConsolidatedEvidence(
            command.MonitoringRecordIds,
            command.AlertIds,
            command.IncidentIds,
            command.FieldEvidenceIds);

        report.Generate(evidence, ComputeSha256(evidence), command.DigitalSignature);
        await _reports.UpdateAsync(report, cancellationToken);
        return report;
    }

    private async Task<EnvironmentalStatus> UpdateStatusAsync(
        ComplianceScope scope,
        string subjectId,
        EnvironmentalStanding standing,
        CancellationToken cancellationToken)
    {
        var current = await _statuses.GetBySubjectAsync(scope, subjectId, cancellationToken);
        if (current == null)
        {
            var created = new EnvironmentalStatus(scope, subjectId, standing);
            await _statuses.AddAsync(created, cancellationToken);
            return created;
        }

        current.Update(standing);
        await _statuses.UpdateAsync(current, cancellationToken);
        return current;
    }

    private static string ComputeSha256(ConsolidatedEvidence evidence)
    {
        var canonical = string.Join(
            "|",
            JoinIds("monitoring", evidence.MonitoringRecordIds),
            JoinIds("alerts", evidence.AlertIds),
            JoinIds("incidents", evidence.IncidentIds),
            JoinIds("evidence", evidence.FieldEvidenceIds));

        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(canonical));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private static string JoinIds(string name, IReadOnlyList<string> ids)
    {
        return name + ":" + string.Join(",", ids.OrderBy(id => id, StringComparer.Ordinal));
    }
}
