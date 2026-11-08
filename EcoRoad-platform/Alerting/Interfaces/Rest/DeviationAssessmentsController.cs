using System.Net.Mime;
using EcoRoad.Alerting.Application.CommandServices;
using EcoRoad.Alerting.Application.QueryServices;
using EcoRoad.Alerting.Domain.Model;
using EcoRoad.Alerting.Domain.Model.Commands;
using EcoRoad.Alerting.Domain.Model.Queries;
using EcoRoad.Alerting.Interfaces.Rest.Resources;
using EcoRoad.Alerting.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EcoRoad.Alerting.Interfaces.Rest;

/// <summary>
///     HTTP API for threshold deviation assessments.
/// </summary>
[ApiController]
[Route("api/v1/deviation-assessments")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Deviation assessments")]
public class DeviationAssessmentsController : ControllerBase
{
    private readonly IAlertingCommandService _commands;
    private readonly IAlertingQueryService _queries;

    public DeviationAssessmentsController(IAlertingCommandService commands, IAlertingQueryService queries)
    {
        _commands = commands;
        _queries = queries;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get deviation assessments", OperationId = "GetDeviationAssessments")]
    [SwaggerResponse(StatusCodes.Status200OK, "Assessments found.", typeof(IEnumerable<DeviationAssessmentResource>))]
    public async Task<IActionResult> GetDeviationAssessments(
        [FromQuery] string? measurementId,
        CancellationToken cancellationToken)
    {
        var assessments = await _queries.Handle(new GetDeviationAssessmentsQuery(measurementId), cancellationToken);
        return Ok(DeviationAssessmentResourceFromEntityAssembler.ToResourcesFromViews(assessments));
    }

    [HttpGet("{assessmentId}")]
    [SwaggerOperation(Summary = "Get a deviation assessment by id", OperationId = "GetDeviationAssessmentById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Assessment found.", typeof(DeviationAssessmentResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Assessment not found.", typeof(MessageResource))]
    public async Task<IActionResult> GetDeviationAssessmentById(
        string assessmentId,
        CancellationToken cancellationToken)
    {
        var assessment = await _queries.Handle(
            new GetDeviationAssessmentByIdQuery(assessmentId),
            cancellationToken);
        if (assessment == null)
        {
            return NotFound(new MessageResource("Deviation assessment was not found."));
        }

        return Ok(DeviationAssessmentResourceFromEntityAssembler.ToResourceFromView(assessment));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Evaluate a threshold deviation", OperationId = "EvaluateThresholdDeviation")]
    [SwaggerResponse(StatusCodes.Status200OK, "Measurement is within the threshold.", typeof(ThresholdEvaluationResource))]
    [SwaggerResponse(StatusCodes.Status201Created, "Deviation detected and alert raised.", typeof(ThresholdEvaluationResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The measurement cannot be evaluated.", typeof(MessageResource))]
    public async Task<IActionResult> EvaluateThresholdDeviation(
        [FromBody] EvaluateThresholdDeviationResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = EvaluateThresholdDeviationCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await _commands.Handle(command, cancellationToken);
            var body = ThresholdEvaluationResourceFromResultAssembler.ToResourceFromResult(result);
            if (result.Alert == null)
            {
                return Ok(body);
            }

            return CreatedAtAction(
                nameof(GetDeviationAssessmentById),
                new { assessmentId = result.Assessment.Id },
                body);
        }
        catch (AlertingRuleException exception)
        {
            return MapRuleException(exception);
        }
    }

    [HttpPost("{assessmentId}/confirmation")]
    [SwaggerOperation(Summary = "Confirm a critical deviation", OperationId = "ConfirmCriticalDeviation")]
    [SwaggerResponse(StatusCodes.Status200OK, "Deviation confirmed as critical.", typeof(DeviationAssessmentResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The measurement is within the threshold.", typeof(MessageResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Assessment not found.", typeof(MessageResource))]
    public async Task<IActionResult> ConfirmCriticalDeviation(
        string assessmentId,
        CancellationToken cancellationToken)
    {
        try
        {
            var assessment = await _commands.Handle(
                new ConfirmCriticalDeviationCommand(assessmentId),
                cancellationToken);
            return Ok(DeviationAssessmentResourceFromEntityAssembler.ToResourceFromEntity(assessment));
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
