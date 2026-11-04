using EcoRoad.Monitoring.Application.QueryServices;
using EcoRoad.Monitoring.Domain.Model.Queries;
using EcoRoad.Monitoring.Domain.Model.ValueObjects;
using EcoRoad.Monitoring.Interfaces.Rest.Resources;
using EcoRoad.Monitoring.Interfaces.Rest.Transform;
using EcoRoad.Shared.Interfaces.Rest.Resources;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoad.Monitoring.Interfaces.Rest;

/// <summary>
/// Read endpoints with the environmental situation and the history of a road project.
/// </summary>
[ApiController]
[Route("api/monitoring/projects/{projectId}")]
[Produces("application/json")]
public class MonitoringProjectsController : ControllerBase
{
    private readonly IEnvironmentalMeasurementQueryService _queries;

    /// <summary>
    /// Initializes a new instance of the <see cref="MonitoringProjectsController" /> class.
    /// </summary>
    /// <param name="queries">Query service of measurements.</param>
    public MonitoringProjectsController(IEnvironmentalMeasurementQueryService queries)
    {
        _queries = queries;
    }

    /// <summary>
    /// Returns the current indicators, latest readings and status of every section of a project.
    /// </summary>
    /// <param name="projectId">Identifier of the road project.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The project dashboard.</returns>
    [HttpGet("dashboard")]
    [ProducesResponseType(typeof(MonitoringDashboardResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDashboard(string projectId, CancellationToken cancellationToken)
    {
        var dashboard = await _queries.Handle(new GetMonitoringDashboardQuery(projectId), cancellationToken);
        return Ok(MeasurementAssembler.ToDashboardResource(dashboard));
    }

    /// <summary>
    /// Returns the history of a parameter, oldest first.
    /// </summary>
    /// <param name="projectId">Identifier of the road project.</param>
    /// <param name="parameterType">The environmental parameter.</param>
    /// <param name="sectionId">Optional road section filter.</param>
    /// <param name="from">Optional first day, format <c>yyyy-MM-dd</c>.</param>
    /// <param name="to">Optional last day, format <c>yyyy-MM-dd</c>.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The filtered measurements.</returns>
    [HttpGet("history")]
    [ProducesResponseType(typeof(IReadOnlyList<MeasurementResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> GetHistory(
        string projectId,
        [FromQuery] ParameterType parameterType,
        [FromQuery] string? sectionId,
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        CancellationToken cancellationToken)
    {
        var query = new GetMeasurementHistoryQuery(projectId, sectionId, parameterType, from, to);
        var history = await _queries.Handle(query, cancellationToken);
        return Ok(history.Select(MeasurementAssembler.ToResourceFromEntity).ToList());
    }
}
