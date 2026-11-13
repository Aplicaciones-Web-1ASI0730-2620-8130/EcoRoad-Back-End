using EcoRoad.Compliance.Domain.Model.Aggregates;
using EcoRoad.Compliance.Domain.Model.Commands;
using EcoRoad.Compliance.Domain.Model.Queries;
using EcoRoad.Compliance.Interfaces.Rest.Resources;

namespace EcoRoad.Compliance.Interfaces.Rest.Transform;

public static class UpdateEnvironmentalStatusCommandFromResourceAssembler
{
    public static UpdateProjectEnvironmentalStatusCommand ToProjectCommandFromResource(
        string projectId,
        UpdateEnvironmentalStatusResource resource)
    {
        return new UpdateProjectEnvironmentalStatusCommand(projectId, resource.Standing);
    }

    public static UpdatePortfolioEnvironmentalStatusCommand ToPortfolioCommandFromResource(
        string portfolioId,
        UpdateEnvironmentalStatusResource resource)
    {
        return new UpdatePortfolioEnvironmentalStatusCommand(portfolioId, resource.Standing);
    }
}

public static class RequestComplianceReportCommandFromResourceAssembler
{
    public static RequestComplianceReportCommand ToCommandFromResource(RequestComplianceReportResource resource)
    {
        return new RequestComplianceReportCommand(resource.Scope, resource.SubjectId, resource.RequestedBy);
    }
}

public static class GenerateComplianceReportCommandFromResourceAssembler
{
    public static GenerateComplianceReportCommand ToCommandFromResource(
        string reportId,
        GenerateComplianceReportResource resource)
    {
        return new GenerateComplianceReportCommand(
            reportId,
            resource.MonitoringRecordIds ?? Array.Empty<string>(),
            resource.AlertIds ?? Array.Empty<string>(),
            resource.IncidentIds ?? Array.Empty<string>(),
            resource.FieldEvidenceIds ?? Array.Empty<string>(),
            resource.DigitalSignature);
    }
}

public static class EnvironmentalStatusResourceFromEntityAssembler
{
    public static EnvironmentalStatusResource ToResourceFromEntity(EnvironmentalStatus status)
    {
        return new EnvironmentalStatusResource(
            status.Id,
            status.Scope,
            status.SubjectId,
            status.Standing,
            status.UpdatedAt);
    }
}

public static class ComplianceReportResourceFromViewAssembler
{
    public static ComplianceReportResource ToResourceFromView(ComplianceReportView view)
    {
        return new ComplianceReportResource(
            view.Id,
            view.Scope,
            view.SubjectId,
            view.RequestedBy,
            view.Status,
            view.RequestedAt,
            view.GeneratedAt,
            view.ContentHash,
            view.DigitalSignature,
            view.MonitoringRecordIds,
            view.AlertIds,
            view.IncidentIds,
            view.FieldEvidenceIds,
            view.IsSealed);
    }
}
