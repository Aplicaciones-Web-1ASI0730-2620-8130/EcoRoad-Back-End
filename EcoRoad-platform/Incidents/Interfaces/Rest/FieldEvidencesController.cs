using EcoRoad.Incidents.Application.CommandServices;
using EcoRoad.Incidents.Application.QueryServices;
using EcoRoad.Incidents.Domain.Model;
using EcoRoad.Incidents.Domain.Model.Queries;
using EcoRoad.Incidents.Interfaces.Rest.Resources;
using EcoRoad.Incidents.Interfaces.Rest.Transform;
using EcoRoad.Shared.Domain.Model.Exceptions;
using EcoRoad.Shared.Interfaces.Rest.Resources;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoad.Incidents.Interfaces.Rest;

/// <summary>
/// Endpoints to record and consult the field evidence of an incident.
/// </summary>
[ApiController]
[Route("api/incidents/{incidentId}/evidence")]
[Produces("application/json")]
public class FieldEvidencesController : ControllerBase
{
    private readonly IEnvironmentalIncidentCommandService _commands;
    private readonly IEnvironmentalIncidentQueryService _queries;

    /// <summary>
    /// Initializes a new instance of the <see cref="FieldEvidencesController" /> class.
    /// </summary>
    /// <param name="commands">Command service of incidents.</param>
    /// <param name="queries">Query service of incidents.</param>
    public FieldEvidencesController(IEnvironmentalIncidentCommandService commands, IEnvironmentalIncidentQueryService queries)
    {
        _commands = commands;
        _queries = queries;
    }

    /// <summary>
    /// Lists the field evidence of an incident, oldest first.
    /// </summary>
    /// <param name="incidentId">Identifier of the incident.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The field evidence.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<FieldEvidenceResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEvidence(string incidentId, CancellationToken cancellationToken)
    {
        var incident = await _queries.Handle(new GetIncidentByIdQuery(incidentId), cancellationToken)
            ?? throw new IncidentsRuleException("INCIDENT_NOT_FOUND", "The incident was not found.", DomainErrorKind.NotFound);
        return Ok(incident.FieldEvidences
            .OrderBy(evidence => evidence.RecordedAt)
            .Select(IncidentResourceAssembler.ToEvidenceResource)
            .ToList());
    }

    /// <summary>
    /// Records a piece of field evidence for an assigned incident.
    /// </summary>
    /// <param name="incidentId">Identifier of the incident.</param>
    /// <param name="resource">Notes and optional file address.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The recorded evidence.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(FieldEvidenceResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> RecordEvidence(
        string incidentId,
        [FromBody] RecordFieldEvidenceResource resource,
        CancellationToken cancellationToken)
    {
        var evidence = await _commands.Handle(
            IncidentCommandFromResourceAssembler.ToEvidenceCommand(incidentId, resource),
            cancellationToken);
        return Created(
            $"/api/incidents/{incidentId}/evidence/{evidence.Id}",
            IncidentResourceAssembler.ToEvidenceResource(evidence));
    }
}
