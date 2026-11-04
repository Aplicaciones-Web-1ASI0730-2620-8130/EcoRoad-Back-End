using EcoRoad.Assets.Application.CommandServices;
using EcoRoad.Assets.Application.QueryServices;
using EcoRoad.Assets.Domain.Model;
using EcoRoad.Assets.Domain.Model.Commands;
using EcoRoad.Assets.Domain.Model.Queries;
using EcoRoad.Assets.Interfaces.Rest.Resources;
using EcoRoad.Assets.Interfaces.Rest.Transform;
using EcoRoad.Shared.Domain.Model.Exceptions;
using EcoRoad.Shared.Interfaces.Rest.Resources;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoad.Assets.Interfaces.Rest;

/// <summary>
/// Endpoints to register, consult and maintain geolocated monitoring points.
/// </summary>
[ApiController]
[Route("api/assets/points")]
[Produces("application/json")]
public class MonitoringPointsController : ControllerBase
{
    private readonly IMonitoringPointCommandService _commands;
    private readonly IMonitoringPointQueryService _queries;

    /// <summary>
    /// Initializes a new instance of the <see cref="MonitoringPointsController" /> class.
    /// </summary>
    /// <param name="commands">Command service of monitoring points.</param>
    /// <param name="queries">Query service of monitoring points.</param>
    public MonitoringPointsController(IMonitoringPointCommandService commands, IMonitoringPointQueryService queries)
    {
        _commands = commands;
        _queries = queries;
    }

    /// <summary>
    /// Lists monitoring points ordered by project and kilometer post.
    /// </summary>
    /// <param name="projectId">Optional road project filter.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The monitoring points.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<MonitoringPointResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPoints([FromQuery] string? projectId, CancellationToken cancellationToken)
    {
        var points = await _queries.Handle(new GetMonitoringPointsQuery(projectId), cancellationToken);
        return Ok(MonitoringPointResourceFromEntityAssembler.ToResourcesFromEntities(points));
    }

    /// <summary>
    /// Returns a monitoring point.
    /// </summary>
    /// <param name="pointId">Identifier of the point.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The monitoring point.</returns>
    [HttpGet("{pointId}")]
    [ProducesResponseType(typeof(MonitoringPointResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPoint(string pointId, CancellationToken cancellationToken)
    {
        var point = await _queries.Handle(new GetMonitoringPointByIdQuery(pointId), cancellationToken)
            ?? throw new AssetsRuleException("POINT_NOT_FOUND", "The monitoring point was not found.", DomainErrorKind.NotFound);
        return Ok(MonitoringPointResourceFromEntityAssembler.ToResourceFromEntity(point));
    }

    /// <summary>
    /// Registers a geolocated monitoring point.
    /// </summary>
    /// <param name="resource">Data of the point.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The new point.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(MonitoringPointResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> RegisterPoint(
        [FromBody] SaveMonitoringPointResource resource,
        CancellationToken cancellationToken)
    {
        var point = await _commands.Handle(
            MonitoringPointCommandFromResourceAssembler.ToRegisterCommand(resource),
            cancellationToken);
        return CreatedAtAction(
            nameof(GetPoint),
            new { pointId = point.Id },
            MonitoringPointResourceFromEntityAssembler.ToResourceFromEntity(point));
    }

    /// <summary>
    /// Updates a monitoring point. Only allowed while no sensor is installed.
    /// </summary>
    /// <param name="pointId">Identifier of the point.</param>
    /// <param name="resource">New data of the point.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The updated point.</returns>
    [HttpPut("{pointId}")]
    [ProducesResponseType(typeof(MonitoringPointResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdatePoint(
        string pointId,
        [FromBody] SaveMonitoringPointResource resource,
        CancellationToken cancellationToken)
    {
        var point = await _commands.Handle(
            MonitoringPointCommandFromResourceAssembler.ToUpdateCommand(pointId, resource),
            cancellationToken);
        return Ok(MonitoringPointResourceFromEntityAssembler.ToResourceFromEntity(point));
    }

    /// <summary>
    /// Deletes a monitoring point that never hosted a sensor.
    /// </summary>
    /// <param name="pointId">Identifier of the point.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>No content.</returns>
    [HttpDelete("{pointId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeletePoint(string pointId, CancellationToken cancellationToken)
    {
        await _commands.Handle(new DeleteMonitoringPointCommand(pointId), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Puts a disabled monitoring point back into service.
    /// </summary>
    /// <param name="pointId">Identifier of the point.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The updated point.</returns>
    [HttpPost("{pointId}/enablement")]
    [ProducesResponseType(typeof(MonitoringPointResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> EnablePoint(string pointId, CancellationToken cancellationToken)
    {
        var point = await _commands.Handle(new EnableMonitoringPointCommand(pointId), cancellationToken);
        return Ok(MonitoringPointResourceFromEntityAssembler.ToResourceFromEntity(point));
    }

    /// <summary>
    /// Takes a monitoring point without sensors out of service.
    /// </summary>
    /// <param name="pointId">Identifier of the point.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The updated point.</returns>
    [HttpPost("{pointId}/disablement")]
    [ProducesResponseType(typeof(MonitoringPointResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DisablePoint(string pointId, CancellationToken cancellationToken)
    {
        var point = await _commands.Handle(new DisableMonitoringPointCommand(pointId), cancellationToken);
        return Ok(MonitoringPointResourceFromEntityAssembler.ToResourceFromEntity(point));
    }
}
