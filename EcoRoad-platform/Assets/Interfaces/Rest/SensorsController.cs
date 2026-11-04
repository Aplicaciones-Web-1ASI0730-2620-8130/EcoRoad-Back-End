using EcoRoad.Assets.Application.CommandServices;
using EcoRoad.Assets.Application.QueryServices;
using EcoRoad.Assets.Domain.Model;
using EcoRoad.Assets.Domain.Model.Commands;
using EcoRoad.Assets.Domain.Model.Queries;
using EcoRoad.Assets.Domain.Model.ValueObjects;
using EcoRoad.Assets.Interfaces.Rest.Resources;
using EcoRoad.Assets.Interfaces.Rest.Transform;
using EcoRoad.Shared.Domain.Model.Exceptions;
using EcoRoad.Shared.Interfaces.Rest.Resources;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoad.Assets.Interfaces.Rest;

/// <summary>
/// Endpoints to manage the sensor inventory and its deployment lifecycle.
/// </summary>
[ApiController]
[Route("api/assets/sensors")]
[Produces("application/json")]
public class SensorsController : ControllerBase
{
    private readonly ISensorCommandService _commands;
    private readonly ISensorQueryService _queries;

    /// <summary>
    /// Initializes a new instance of the <see cref="SensorsController" /> class.
    /// </summary>
    /// <param name="commands">Command service of sensors.</param>
    /// <param name="queries">Query service of sensors.</param>
    public SensorsController(ISensorCommandService commands, ISensorQueryService queries)
    {
        _commands = commands;
        _queries = queries;
    }

    /// <summary>
    /// Lists the sensor inventory.
    /// </summary>
    /// <param name="projectId">Optional road project filter.</param>
    /// <param name="status">Optional deployment status filter.</param>
    /// <param name="search">Optional text matched against the code, name and serial number.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The matching sensors.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<SensorResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSensors(
        [FromQuery] string? projectId,
        [FromQuery] SensorStatus? status,
        [FromQuery] string? search,
        CancellationToken cancellationToken)
    {
        var sensors = await _queries.Handle(new GetSensorsQuery(projectId, status, search), cancellationToken);
        return Ok(SensorResourceFromEntityAssembler.ToResourcesFromEntities(sensors));
    }

    /// <summary>
    /// Returns a sensor with its reporting policy evaluated now.
    /// </summary>
    /// <param name="sensorId">Identifier of the sensor.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The sensor.</returns>
    [HttpGet("{sensorId}")]
    [ProducesResponseType(typeof(SensorResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSensor(string sensorId, CancellationToken cancellationToken)
    {
        var sensor = await _queries.Handle(new GetSensorByIdQuery(sensorId), cancellationToken)
            ?? throw new AssetsRuleException("SENSOR_NOT_FOUND", "The sensor was not found.", DomainErrorKind.NotFound);
        return Ok(SensorResourceFromEntityAssembler.ToResourceFromEntity(sensor));
    }

    /// <summary>
    /// Returns the installation history of a sensor.
    /// </summary>
    /// <param name="sensorId">Identifier of the sensor.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The assignments, newest first.</returns>
    [HttpGet("{sensorId}/assignments")]
    [ProducesResponseType(typeof(IReadOnlyList<SensorAssignmentResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAssignments(string sensorId, CancellationToken cancellationToken)
    {
        var assignments = await _queries.Handle(new GetSensorAssignmentsQuery(sensorId), cancellationToken);
        return Ok(assignments.Select(SensorResourceFromEntityAssembler.ToAssignmentResource).ToList());
    }

    /// <summary>
    /// Registers a sensor in stock.
    /// </summary>
    /// <param name="resource">Data of the sensor.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The new sensor.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(SensorResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> RegisterSensor(
        [FromBody] RegisterSensorResource resource,
        CancellationToken cancellationToken)
    {
        var sensor = await _commands.Handle(SensorCommandFromResourceAssembler.ToRegisterCommand(resource), cancellationToken);
        return CreatedAtAction(
            nameof(GetSensor),
            new { sensorId = sensor.Id },
            SensorResourceFromEntityAssembler.ToResourceFromEntity(sensor));
    }

    /// <summary>
    /// Installs a sensor at a monitoring point. Any previous calibration becomes invalid.
    /// </summary>
    /// <param name="sensorId">Identifier of the sensor.</param>
    /// <param name="resource">Destination point.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The updated sensor.</returns>
    [HttpPost("{sensorId}/assignment")]
    [ProducesResponseType(typeof(SensorResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AssignSensor(
        string sensorId,
        [FromBody] AssignSensorResource resource,
        CancellationToken cancellationToken)
    {
        var sensor = await _commands.Handle(
            SensorCommandFromResourceAssembler.ToAssignCommand(sensorId, resource),
            cancellationToken);
        return Ok(SensorResourceFromEntityAssembler.ToResourceFromEntity(sensor));
    }

    /// <summary>
    /// Registers the calibration of an installed sensor.
    /// </summary>
    /// <param name="sensorId">Identifier of the sensor.</param>
    /// <param name="resource">Validity of the calibration.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The updated sensor.</returns>
    [HttpPost("{sensorId}/calibration")]
    [ProducesResponseType(typeof(SensorResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CalibrateSensor(
        string sensorId,
        [FromBody] CalibrateSensorResource resource,
        CancellationToken cancellationToken)
    {
        var sensor = await _commands.Handle(
            SensorCommandFromResourceAssembler.ToCalibrateCommand(sensorId, resource),
            cancellationToken);
        return Ok(SensorResourceFromEntityAssembler.ToResourceFromEntity(sensor));
    }

    /// <summary>
    /// Authorizes a calibrated sensor to report measurements.
    /// </summary>
    /// <param name="sensorId">Identifier of the sensor.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The updated sensor.</returns>
    [HttpPost("{sensorId}/activation")]
    [ProducesResponseType(typeof(SensorResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ActivateSensor(string sensorId, CancellationToken cancellationToken)
    {
        var sensor = await _commands.Handle(new ActivateSensorCommand(sensorId), cancellationToken);
        return Ok(SensorResourceFromEntityAssembler.ToResourceFromEntity(sensor));
    }

    /// <summary>
    /// Stops an active sensor from reporting while it stays installed.
    /// </summary>
    /// <param name="sensorId">Identifier of the sensor.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The updated sensor.</returns>
    [HttpPost("{sensorId}/deactivation")]
    [ProducesResponseType(typeof(SensorResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeactivateSensor(string sensorId, CancellationToken cancellationToken)
    {
        var sensor = await _commands.Handle(new DeactivateSensorCommand(sensorId), cancellationToken);
        return Ok(SensorResourceFromEntityAssembler.ToResourceFromEntity(sensor));
    }

    /// <summary>
    /// Uninstalls a sensor and returns it to stock.
    /// </summary>
    /// <param name="sensorId">Identifier of the sensor.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The updated sensor.</returns>
    [HttpPost("{sensorId}/release")]
    [ProducesResponseType(typeof(SensorResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ReleaseSensor(string sensorId, CancellationToken cancellationToken)
    {
        var sensor = await _commands.Handle(new ReleaseSensorCommand(sensorId), cancellationToken);
        return Ok(SensorResourceFromEntityAssembler.ToResourceFromEntity(sensor));
    }
}
