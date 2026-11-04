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
/// Endpoints to ingest automatic telemetry and manual field measurements.
/// </summary>
[ApiController]
[Route("api/monitoring/measurements")]
[Produces("application/json")]
public class MeasurementsController : ControllerBase
{
    private readonly IEnvironmentalMeasurementCommandService _commands;
    private readonly IEnvironmentalMeasurementQueryService _queries;

    /// <summary>
    /// Initializes a new instance of the <see cref="MeasurementsController" /> class.
    /// </summary>
    /// <param name="commands">Command service of measurements.</param>
    /// <param name="queries">Query service of measurements.</param>
    public MeasurementsController(
        IEnvironmentalMeasurementCommandService commands,
        IEnvironmentalMeasurementQueryService queries)
    {
        _commands = commands;
        _queries = queries;
    }

    /// <summary>
    /// Returns a measurement.
    /// </summary>
    /// <param name="measurementId">Identifier of the measurement.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The measurement.</returns>
    [HttpGet("{measurementId}")]
    [ProducesResponseType(typeof(MeasurementResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMeasurement(string measurementId, CancellationToken cancellationToken)
    {
        var measurement = await _queries.Handle(new GetMeasurementByIdQuery(measurementId), cancellationToken)
            ?? throw new MonitoringRuleException("MEASUREMENT_NOT_FOUND", "The measurement was not found.", DomainErrorKind.NotFound);
        return Ok(MeasurementAssembler.ToResourceFromEntity(measurement));
    }

    /// <summary>
    /// Captures a reading sent automatically by an IoT sensor.
    /// </summary>
    /// <param name="resource">The sensor reading.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The classified measurement.</returns>
    [HttpPost("telemetry")]
    [ProducesResponseType(typeof(MeasurementResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CaptureTelemetry(
        [FromBody] CaptureTelemetryReadingResource resource,
        CancellationToken cancellationToken)
    {
        var measurement = await _commands.Handle(MeasurementAssembler.ToTelemetryCommand(resource), cancellationToken);
        return CreatedAtAction(
            nameof(GetMeasurement),
            new { measurementId = measurement.Id },
            MeasurementAssembler.ToResourceFromEntity(measurement));
    }

    /// <summary>
    /// Records a measurement taken manually on site.
    /// </summary>
    /// <param name="resource">The field measurement.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The classified measurement.</returns>
    [HttpPost("manual")]
    [ProducesResponseType(typeof(MeasurementResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> RecordManual(
        [FromBody] RecordManualMeasurementResource resource,
        CancellationToken cancellationToken)
    {
        var measurement = await _commands.Handle(MeasurementAssembler.ToManualCommand(resource), cancellationToken);
        return CreatedAtAction(
            nameof(GetMeasurement),
            new { measurementId = measurement.Id },
            MeasurementAssembler.ToResourceFromEntity(measurement));
    }
}
