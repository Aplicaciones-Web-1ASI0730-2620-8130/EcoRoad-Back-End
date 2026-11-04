using EcoRoad.Monitoring.Application.CommandServices;
using EcoRoad.Monitoring.Application.QueryServices;
using EcoRoad.Monitoring.Domain.Model;
using EcoRoad.Monitoring.Domain.Model.Queries;
using EcoRoad.Monitoring.Interfaces.Rest.Resources;
using EcoRoad.Monitoring.Interfaces.Rest.Transform;
using EcoRoad.Shared.Domain.Model.Exceptions;
using EcoRoad.Shared.Interfaces.Rest.Resources;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoad.Monitoring.Interfaces.Rest;

/// <summary>
/// Endpoints to configure the threshold profiles of the environmental parameters.
/// </summary>
[ApiController]
[Route("api/monitoring/threshold-profiles")]
[Produces("application/json")]
public class ThresholdProfilesController : ControllerBase
{
    private readonly IThresholdProfileCommandService _commands;
    private readonly IThresholdProfileQueryService _queries;

    /// <summary>
    /// Initializes a new instance of the <see cref="ThresholdProfilesController" /> class.
    /// </summary>
    /// <param name="commands">Command service of threshold profiles.</param>
    /// <param name="queries">Query service of threshold profiles.</param>
    public ThresholdProfilesController(IThresholdProfileCommandService commands, IThresholdProfileQueryService queries)
    {
        _commands = commands;
        _queries = queries;
    }

    /// <summary>
    /// Lists every threshold profile.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The profiles ordered by parameter.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ThresholdProfileResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfiles(CancellationToken cancellationToken)
    {
        var profiles = await _queries.Handle(new GetThresholdProfilesQuery(), cancellationToken);
        return Ok(profiles.Select(ThresholdProfileAssembler.ToResourceFromEntity).ToList());
    }

    /// <summary>
    /// Returns a threshold profile.
    /// </summary>
    /// <param name="profileId">Identifier of the profile.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The profile.</returns>
    [HttpGet("{profileId}")]
    [ProducesResponseType(typeof(ThresholdProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile(string profileId, CancellationToken cancellationToken)
    {
        var profile = await _queries.Handle(new GetThresholdProfileByIdQuery(profileId), cancellationToken)
            ?? throw new MonitoringRuleException("PROFILE_NOT_FOUND", "The threshold profile was not found.", DomainErrorKind.NotFound);
        return Ok(ThresholdProfileAssembler.ToResourceFromEntity(profile));
    }

    /// <summary>
    /// Configures the threshold profile of a parameter.
    /// </summary>
    /// <param name="resource">Parameter and limits of the profile.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The new profile.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ThresholdProfileResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ConfigureProfile(
        [FromBody] ConfigureThresholdProfileResource resource,
        CancellationToken cancellationToken)
    {
        var profile = await _commands.Handle(ThresholdProfileAssembler.ToConfigureCommand(resource), cancellationToken);
        return CreatedAtAction(
            nameof(GetProfile),
            new { profileId = profile.Id },
            ThresholdProfileAssembler.ToResourceFromEntity(profile));
    }

    /// <summary>
    /// Replaces the limits of a threshold profile. Past measurements keep their original status.
    /// </summary>
    /// <param name="profileId">Identifier of the profile.</param>
    /// <param name="resource">The new limits.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The updated profile.</returns>
    [HttpPut("{profileId}/thresholds")]
    [ProducesResponseType(typeof(ThresholdProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> AssignThresholds(
        string profileId,
        [FromBody] ThresholdLimitsResource resource,
        CancellationToken cancellationToken)
    {
        var profile = await _commands.Handle(ThresholdProfileAssembler.ToAssignCommand(profileId, resource), cancellationToken);
        return Ok(ThresholdProfileAssembler.ToResourceFromEntity(profile));
    }
}
