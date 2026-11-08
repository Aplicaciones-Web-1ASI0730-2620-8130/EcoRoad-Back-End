using System.Net.Mime;
using EcoRoad.Alerting.Application.CommandServices;
using EcoRoad.Alerting.Application.QueryServices;
using EcoRoad.Alerting.Domain.Model;
using EcoRoad.Alerting.Domain.Model.Commands;
using EcoRoad.Alerting.Domain.Model.Queries;
using EcoRoad.Alerting.Domain.Model.ValueObjects;
using EcoRoad.Alerting.Interfaces.Rest.Resources;
using EcoRoad.Alerting.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EcoRoad.Alerting.Interfaces.Rest;

/// <summary>
///     HTTP API for environmental alerts.
/// </summary>
[ApiController]
[Route("api/v1/alerts")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Environmental alerts")]
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
    [SwaggerOperation(Summary = "Get alerts", OperationId = "GetAlerts")]
    [SwaggerResponse(StatusCodes.Status200OK, "Alerts found.", typeof(IEnumerable<AlertResource>))]
    public async Task<IActionResult> GetAlerts(
        [FromQuery] string? monitoringPointId,
        [FromQuery] AlertStatus? status,
        CancellationToken cancellationToken)
    {
        var alerts = await _queries.Handle(new GetAlertsQuery(monitoringPointId, status), cancellationToken);
        return Ok(AlertResourceFromEntityAssembler.ToResourcesFromViews(alerts));
    }

    [HttpGet("active")]
    [SwaggerOperation(Summary = "Get raised alerts", OperationId = "GetActiveAlerts")]
    [SwaggerResponse(StatusCodes.Status200OK, "Raised alerts found.", typeof(IEnumerable<AlertResource>))]
    public async Task<IActionResult> GetActiveAlerts(CancellationToken cancellationToken)
    {
        var alerts = await _queries.Handle(new GetActiveAlertsQuery(), cancellationToken);
        return Ok(AlertResourceFromEntityAssembler.ToResourcesFromViews(alerts));
    }

    [HttpGet("{alertId}")]
    [SwaggerOperation(Summary = "Get an alert by id", OperationId = "GetAlertById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Alert found.", typeof(AlertResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Alert not found.", typeof(MessageResource))]
    public async Task<IActionResult> GetAlertById(string alertId, CancellationToken cancellationToken)
    {
        var alert = await _queries.Handle(new GetAlertByIdQuery(alertId), cancellationToken);
        if (alert == null)
        {
            return NotFound(new MessageResource("Alert was not found."));
        }

        return Ok(AlertResourceFromEntityAssembler.ToResourceFromView(alert));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Raise an environmental alert", OperationId = "RaiseEnvironmentalAlert")]
    [SwaggerResponse(StatusCodes.Status201Created, "Alert raised.", typeof(AlertResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The assessment is not a deviation.", typeof(MessageResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Deviation assessment not found.", typeof(MessageResource))]
    public async Task<IActionResult> RaiseEnvironmentalAlert(
        [FromBody] RaiseEnvironmentalAlertResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = RaiseEnvironmentalAlertCommandFromResourceAssembler.ToCommandFromResource(resource);
            var alert = await _commands.Handle(command, cancellationToken);
            return CreatedAtAction(
                nameof(GetAlertById),
                new { alertId = alert.Id },
                AlertResourceFromEntityAssembler.ToResourceFromEntity(alert));
        }
        catch (AlertingRuleException exception)
        {
            return MapRuleException(exception);
        }
    }

    [HttpPost("{alertId}/acknowledgement")]
    [SwaggerOperation(Summary = "Acknowledge an alert", OperationId = "AcknowledgeAlert")]
    [SwaggerResponse(StatusCodes.Status200OK, "Alert acknowledged.", typeof(AlertResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The alert cannot be acknowledged.", typeof(MessageResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Alert not found.", typeof(MessageResource))]
    public async Task<IActionResult> AcknowledgeAlert(
        string alertId,
        [FromBody] AcknowledgeAlertResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var alert = await _commands.Handle(
                new AcknowledgeAlertCommand(alertId, resource.UserId),
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
            return NotFound(new MessageResource(exception.Message));
        }

        return BadRequest(new MessageResource(exception.Message));
    }
}
