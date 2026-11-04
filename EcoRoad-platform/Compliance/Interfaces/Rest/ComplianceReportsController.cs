using EcoRoad.Compliance.Application.CommandServices;
using EcoRoad.Compliance.Application.QueryServices;
using EcoRoad.Compliance.Domain.Model;
using EcoRoad.Compliance.Domain.Model.Queries;
using EcoRoad.Compliance.Interfaces.Rest.Documents;
using EcoRoad.Compliance.Interfaces.Rest.Resources;
using EcoRoad.Compliance.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoad.Compliance.Interfaces.Rest;

[ApiController]
[Route("api/compliance-reports")]
public class ComplianceReportsController : ControllerBase
{
    private readonly IComplianceCommandService _commands;
    private readonly IComplianceQueryService _queries;

    public ComplianceReportsController(IComplianceCommandService commands, IComplianceQueryService queries)
    {
        _commands = commands;
        _queries = queries;
    }

    [HttpPost]
    public async Task<IActionResult> RequestComplianceReport(
        [FromBody] RequestComplianceReportResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = RequestComplianceReportCommandFromResourceAssembler.ToCommandFromResource(resource);
            var report = await _commands.Handle(command, cancellationToken);
            var view = await _queries.Handle(new GetComplianceReportViewQuery(report.Id), cancellationToken);
            return CreatedAtAction(
                nameof(GetComplianceReport),
                new { reportId = report.Id },
                ComplianceReportResourceFromViewAssembler.ToResourceFromView(view!));
        }
        catch (ComplianceRuleException exception)
        {
            return MapRuleException(exception);
        }
    }

    [HttpGet("{reportId}")]
    public async Task<IActionResult> GetComplianceReport(string reportId, CancellationToken cancellationToken)
    {
        var view = await _queries.Handle(new GetComplianceReportViewQuery(reportId), cancellationToken);
        if (view == null)
        {
            return NotFound(new { message = "Compliance report was not found." });
        }

        return Ok(ComplianceReportResourceFromViewAssembler.ToResourceFromView(view));
    }

    [HttpPost("{reportId}/generation")]
    public async Task<IActionResult> GenerateComplianceReport(
        string reportId,
        [FromBody] GenerateComplianceReportResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = GenerateComplianceReportCommandFromResourceAssembler.ToCommandFromResource(reportId, resource);
            await _commands.Handle(command, cancellationToken);
            var view = await _queries.Handle(new GetComplianceReportViewQuery(reportId), cancellationToken);
            return Ok(ComplianceReportResourceFromViewAssembler.ToResourceFromView(view!));
        }
        catch (ComplianceRuleException exception)
        {
            return MapRuleException(exception);
        }
    }

    [HttpGet("{reportId}/document")]
    public async Task<IActionResult> DownloadComplianceReport(string reportId, CancellationToken cancellationToken)
    {
        var view = await _queries.Handle(new GetComplianceReportViewQuery(reportId), cancellationToken);
        if (view == null)
        {
            return NotFound(new { message = "Compliance report was not found." });
        }

        if (!view.IsSealed)
        {
            return BadRequest(new { message = "The compliance report has not been generated." });
        }

        var document = ComplianceReportDocument.Create(view);
        return File(document, "application/pdf", "compliance-report-" + view.Id + ".pdf");
    }

    private IActionResult MapRuleException(ComplianceRuleException exception)
    {
        if (exception.NotFound)
        {
            return NotFound(new { message = exception.Message });
        }

        return BadRequest(new { message = exception.Message });
    }
}
