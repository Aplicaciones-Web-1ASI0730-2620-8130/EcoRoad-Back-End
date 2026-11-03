using EcoRoad.Alerting.Application.CommandServices;
using EcoRoad.Alerting.Application.QueryServices;
using EcoRoad.Alerting.Domain.Model;
using EcoRoad.Alerting.Domain.Model.Commands;
using EcoRoad.Alerting.Domain.Model.Queries;
using EcoRoad.Alerting.Interfaces.Rest.Resources;
using EcoRoad.Alerting.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoad.Alerting.Interfaces.Rest;

[ApiController]
[Route("api/alert-rules")]
public class AlertRulesController : ControllerBase
{
    private readonly IAlertingCommandService _commands;

    public AlertRulesController(IAlertingCommandService commands)
    {
        _commands = commands;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterAlertRule(
        [FromBody] RegisterAlertRuleResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = RegisterAlertRuleCommandFromResourceAssembler.ToCommandFromResource(resource);
            var rule = await _commands.Handle(command, cancellationToken);
            return Created($"/api/alert-rules/{rule.Id}", AlertRuleResourceFromEntityAssembler.ToResourceFromEntity(rule));
        }
        catch (AlertingRuleException exception)
        {
            return MapRuleException(exception);
        }
    }

    private IActionResult MapRuleException(AlertingRuleException exception)
    {
        if (exception.NotFound)
        {
            return NotFound(new { message = exception.Message });
        }

        return BadRequest(new { message = exception.Message });
    }
}
