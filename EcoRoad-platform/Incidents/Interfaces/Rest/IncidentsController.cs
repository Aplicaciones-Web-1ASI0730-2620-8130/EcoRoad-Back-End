using EcoRoad.Incidents.Application.CommandServices;
using EcoRoad.Incidents.Application.QueryServices;
using EcoRoad.Incidents.Domain.Model;
using EcoRoad.Incidents.Domain.Model.Commands;
using EcoRoad.Incidents.Domain.Model.Queries;
using EcoRoad.Incidents.Domain.Model.ValueObjects;
using EcoRoad.Incidents.Interfaces.Rest.Resources;
using EcoRoad.Incidents.Interfaces.Rest.Transform;
using EcoRoad.Shared.Domain.Model.Exceptions;
using EcoRoad.Shared.Interfaces.Rest.Resources;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoad.Incidents.Interfaces.Rest;

/// <summary>
/// Endpoints of the incident board: opening, assignment, attention and closure of incidents.
/// </summary>
[ApiController]
[Route("api/incidents")]
[Produces("application/json")]
public class IncidentsController : ControllerBase
{
    private readonly IEnvironmentalIncidentCommandService _commands;
    private readonly IEnvironmentalIncidentQueryService _queries;

    /// <summary>
    /// Initializes a new instance of the <see cref="IncidentsController" /> class.
    /// </summary>
    /// <param name="commands">Command service of incidents.</param>
    /// <param name="queries">Query service of incidents.</param>
    public IncidentsController(IEnvironmentalIncidentCommandService commands, IEnvironmentalIncidentQueryService queries)
    {
        _commands = commands;
        _queries = queries;
    }

    /// <summary>
    /// Lists the incidents of the board ordered by status and newest first.
    /// </summary>
    /// <param name="companyId">Optional company filter.</param>
    /// <param name="status">Optional status filter.</param>
    /// <param name="projectId">Optional road project filter.</param>
    /// <param name="search">Optional text matched against the code and the title.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The matching incidents.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<IncidentResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIncidents(
        [FromQuery] string? companyId,
        [FromQuery] IncidentStatus? status,
        [FromQuery] string? projectId,
        [FromQuery] string? search,
        CancellationToken cancellationToken)
    {
        var incidents = await _queries.Handle(new GetIncidentsQuery(companyId, status, projectId, search), cancellationToken);
        return Ok(incidents.Select(IncidentResourceAssembler.ToResourceFromEntity).ToList());
    }

    /// <summary>
    /// Counts the incidents in every column of the board.
    /// </summary>
    /// <param name="companyId">Optional company filter.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The board summary.</returns>
    [HttpGet("summary")]
    [ProducesResponseType(typeof(IncidentBoardSummaryResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary([FromQuery] string? companyId, CancellationToken cancellationToken)
    {
        var summary = await _queries.Handle(new GetIncidentBoardSummaryQuery(companyId), cancellationToken);
        return Ok(IncidentResourceAssembler.ToSummaryResource(summary));
    }

    /// <summary>
    /// Returns an incident with its corrective actions and evidence.
    /// </summary>
    /// <param name="incidentId">Identifier of the incident.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The incident.</returns>
    [HttpGet("{incidentId}")]
    [ProducesResponseType(typeof(IncidentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetIncident(string incidentId, CancellationToken cancellationToken)
    {
        var incident = await _queries.Handle(new GetIncidentByIdQuery(incidentId), cancellationToken)
            ?? throw new IncidentsRuleException("INCIDENT_NOT_FOUND", "The incident was not found.", DomainErrorKind.NotFound);
        return Ok(IncidentResourceAssembler.ToResourceFromEntity(incident));
    }

    /// <summary>
    /// Opens an incident from an alert. An alert can originate only one incident.
    /// </summary>
    /// <param name="resource">Data of the incident.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The open incident.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(IncidentResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> OpenIncident([FromBody] OpenIncidentResource resource, CancellationToken cancellationToken)
    {
        var incident = await _commands.Handle(IncidentCommandFromResourceAssembler.ToOpenCommand(resource), cancellationToken);
        return CreatedAtAction(
            nameof(GetIncident),
            new { incidentId = incident.Id },
            IncidentResourceAssembler.ToResourceFromEntity(incident));
    }

    /// <summary>
    /// Assigns, or reassigns, the person responsible for the remediation.
    /// </summary>
    /// <param name="incidentId">Identifier of the incident.</param>
    /// <param name="resource">The responsible person.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The updated incident.</returns>
    [HttpPost("{incidentId}/assignment")]
    [ProducesResponseType(typeof(IncidentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AssignResponsible(
        string incidentId,
        [FromBody] AssignResponsibleResource resource,
        CancellationToken cancellationToken)
    {
        var incident = await _commands.Handle(
            IncidentCommandFromResourceAssembler.ToAssignCommand(incidentId, resource),
            cancellationToken);
        return Ok(IncidentResourceAssembler.ToResourceFromEntity(incident));
    }

    /// <summary>
    /// Marks the incident as attended. Requires executed corrective actions and field evidence.
    /// </summary>
    /// <param name="incidentId">Identifier of the incident.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The updated incident.</returns>
    [HttpPost("{incidentId}/resolution")]
    [ProducesResponseType(typeof(IncidentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> MarkAsAttended(string incidentId, CancellationToken cancellationToken)
    {
        var incident = await _commands.Handle(new MarkIncidentAsAttendedCommand(incidentId), cancellationToken);
        return Ok(IncidentResourceAssembler.ToResourceFromEntity(incident));
    }

    /// <summary>
    /// Formally closes an attended incident.
    /// </summary>
    /// <param name="incidentId">Identifier of the incident.</param>
    /// <param name="resource">Closing data.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The closed incident.</returns>
    [HttpPost("{incidentId}/closure")]
    [ProducesResponseType(typeof(IncidentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CloseIncident(
        string incidentId,
        [FromBody] CloseIncidentResource resource,
        CancellationToken cancellationToken)
    {
        var incident = await _commands.Handle(
            IncidentCommandFromResourceAssembler.ToCloseCommand(incidentId, resource),
            cancellationToken);
        return Ok(IncidentResourceAssembler.ToResourceFromEntity(incident));
    }
}
