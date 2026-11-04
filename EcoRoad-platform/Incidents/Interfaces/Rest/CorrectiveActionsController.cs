using EcoRoad.Incidents.Application.CommandServices;
using EcoRoad.Incidents.Domain.Model.Commands;
using EcoRoad.Incidents.Interfaces.Rest.Resources;
using EcoRoad.Incidents.Interfaces.Rest.Transform;
using EcoRoad.Shared.Interfaces.Rest.Resources;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoad.Incidents.Interfaces.Rest;

/// <summary>
/// Endpoints to register and follow up the corrective actions of an incident.
/// </summary>
[ApiController]
[Route("api/incidents/{incidentId}/actions")]
[Produces("application/json")]
public class CorrectiveActionsController : ControllerBase
{
    private readonly ICorrectiveActionCommandService _commands;

    /// <summary>
    /// Initializes a new instance of the <see cref="CorrectiveActionsController" /> class.
    /// </summary>
    /// <param name="commands">Command service of corrective actions.</param>
    public CorrectiveActionsController(ICorrectiveActionCommandService commands)
    {
        _commands = commands;
    }

    /// <summary>
    /// Registers a corrective action for an assigned incident.
    /// </summary>
    /// <param name="incidentId">Identifier of the incident.</param>
    /// <param name="resource">Data of the action.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The new corrective action.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CorrectiveActionResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterAction(
        string incidentId,
        [FromBody] RegisterCorrectiveActionResource resource,
        CancellationToken cancellationToken)
    {
        var action = await _commands.Handle(
            IncidentCommandFromResourceAssembler.ToRegisterActionCommand(incidentId, resource),
            cancellationToken);
        return Created(
            $"/api/incidents/{incidentId}/actions/{action.Id}",
            IncidentResourceAssembler.ToActionResource(action));
    }

    /// <summary>
    /// Starts a corrective action.
    /// </summary>
    /// <param name="incidentId">Identifier of the incident.</param>
    /// <param name="actionId">Identifier of the action.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The updated corrective action.</returns>
    [HttpPost("{actionId}/start")]
    [ProducesResponseType(typeof(CorrectiveActionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> StartAction(string incidentId, string actionId, CancellationToken cancellationToken)
    {
        var action = await _commands.Handle(new StartCorrectiveActionCommand(incidentId, actionId), cancellationToken);
        return Ok(IncidentResourceAssembler.ToActionResource(action));
    }

    /// <summary>
    /// Updates the remediation progress of a corrective action. 100 completes it.
    /// </summary>
    /// <param name="incidentId">Identifier of the incident.</param>
    /// <param name="actionId">Identifier of the action.</param>
    /// <param name="resource">The new progress.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The updated corrective action.</returns>
    [HttpPut("{actionId}/progress")]
    [ProducesResponseType(typeof(CorrectiveActionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UpdateProgress(
        string incidentId,
        string actionId,
        [FromBody] UpdateRemediationProgressResource resource,
        CancellationToken cancellationToken)
    {
        var action = await _commands.Handle(
            IncidentCommandFromResourceAssembler.ToProgressCommand(incidentId, actionId, resource),
            cancellationToken);
        return Ok(IncidentResourceAssembler.ToActionResource(action));
    }

    /// <summary>
    /// Completes a corrective action.
    /// </summary>
    /// <param name="incidentId">Identifier of the incident.</param>
    /// <param name="actionId">Identifier of the action.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The updated corrective action.</returns>
    [HttpPost("{actionId}/execution")]
    [ProducesResponseType(typeof(CorrectiveActionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ExecuteAction(string incidentId, string actionId, CancellationToken cancellationToken)
    {
        var action = await _commands.Handle(new ExecuteCorrectiveActionCommand(incidentId, actionId), cancellationToken);
        return Ok(IncidentResourceAssembler.ToActionResource(action));
    }
}
