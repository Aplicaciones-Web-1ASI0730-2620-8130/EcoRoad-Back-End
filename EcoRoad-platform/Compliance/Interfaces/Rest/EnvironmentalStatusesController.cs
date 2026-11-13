using EcoRoad.Compliance.Application.CommandServices;
using EcoRoad.Compliance.Domain.Model;
using EcoRoad.Compliance.Interfaces.Rest.Resources;
using EcoRoad.Compliance.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoad.Compliance.Interfaces.Rest;

[ApiController]
[Route("api/environmental-statuses")]
public class EnvironmentalStatusesController : ControllerBase
{
    private readonly IComplianceCommandService _commands;

    public EnvironmentalStatusesController(IComplianceCommandService commands)
    {
        _commands = commands;
    }

    [HttpPut("projects/{projectId}")]
    public async Task<IActionResult> UpdateProjectEnvironmentalStatus(
        string projectId,
        [FromBody] UpdateEnvironmentalStatusResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = UpdateEnvironmentalStatusCommandFromResourceAssembler.ToProjectCommandFromResource(
                projectId,
                resource);
            var status = await _commands.Handle(command, cancellationToken);
            return Ok(EnvironmentalStatusResourceFromEntityAssembler.ToResourceFromEntity(status));
        }
        catch (ComplianceRuleException exception)
        {
            return MapRuleException(exception);
        }
    }

    [HttpPut("portfolios/{portfolioId}")]
    public async Task<IActionResult> UpdatePortfolioEnvironmentalStatus(
        string portfolioId,
        [FromBody] UpdateEnvironmentalStatusResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = UpdateEnvironmentalStatusCommandFromResourceAssembler.ToPortfolioCommandFromResource(
                portfolioId,
                resource);
            var status = await _commands.Handle(command, cancellationToken);
            return Ok(EnvironmentalStatusResourceFromEntityAssembler.ToResourceFromEntity(status));
        }
        catch (ComplianceRuleException exception)
        {
            return MapRuleException(exception);
        }
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
