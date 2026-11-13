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
[Route("api/alerts")]
public class AlertsController : ControllerBase
{
    private readonly IAlertingCommandService _commands;
    private readonly IAlertingQueryService _queries;

    public AlertsController(IAlertingCommandService commands, IAlertingQueryService queries)
    {
        _commands = commands;
        _queries = queries;
    }

    [HttpGet]
    public async Task<IActionResult> GetActiveAlerts(CancellationToken cancellationToken)
    {
        var alerts = await _queries.Handle(new GetActiveAlertsQuery(), cancellationToken);
        return Ok(alerts.Select(ActiveAlertResourceFromViewAssembler.ToResourceFromView).ToList());
    }

    [HttpPost("evaluations")]
    public async Task<IActionResult> EvaluateThresholdDeviation(
        [FromBody] EvaluateThresholdDeviationResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = EvaluateThresholdDeviationCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await _commands.Handle(command, cancellationToken);
            var body = ThresholdEvaluationResourceFromResultAssembler.ToResourceFromResult(result);
            if (result.Alert == null)
            {
                return Ok(body);
            }

            return Created($"/api/alerts/{result.Alert.Id}", body);
        }
        catch (AlertingRuleException exception)
        {
            return MapRuleException(exception);
        }
    }

    [HttpPost("{alertId}/acknowledgement")]
    public async Task<IActionResult> AcknowledgeAlert(
        string alertId,
        [FromBody] AcknowledgeAlertResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var alert = await _commands.Handle(
                new AcknowledgeAlertCommand(alertId, resource.AcknowledgedBy),
                cancellationToken);
            return Ok(AlertResourceFromEntityAssembler.ToResourceFromEntity(alert));
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
