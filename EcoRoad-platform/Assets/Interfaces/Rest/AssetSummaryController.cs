using EcoRoad.Assets.Application.QueryServices;
using EcoRoad.Assets.Domain.Model.Queries;
using EcoRoad.Assets.Interfaces.Rest.Resources;
using EcoRoad.Assets.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoad.Assets.Interfaces.Rest;

/// <summary>
/// Endpoint with the deployment indicators of the sensor fleet.
/// </summary>
[ApiController]
[Route("api/assets/summary")]
[Produces("application/json")]
public class AssetSummaryController : ControllerBase
{
    private readonly ISensorQueryService _queries;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssetSummaryController" /> class.
    /// </summary>
    /// <param name="queries">Query service of sensors.</param>
    public AssetSummaryController(ISensorQueryService queries)
    {
        _queries = queries;
    }

    /// <summary>
    /// Returns the totals of sensors ready to report, assigned, available and with calibration due.
    /// </summary>
    /// <param name="projectId">Optional road project filter.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The fleet summary.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(AssetSummaryResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary([FromQuery] string? projectId, CancellationToken cancellationToken)
    {
        var summary = await _queries.Handle(new GetAssetSummaryQuery(projectId), cancellationToken);
        return Ok(SensorResourceFromEntityAssembler.ToSummaryResource(summary));
    }
}
