using EcoRoad.Projects.Application.CommandServices;
using EcoRoad.Projects.Application.QueryServices;
using EcoRoad.Projects.Domain.Model;
using EcoRoad.Projects.Domain.Model.Commands;
using EcoRoad.Projects.Domain.Model.Queries;
using EcoRoad.Projects.Interfaces.Rest.Resources;
using EcoRoad.Projects.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoad.Projects.Interfaces.Rest;

[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly IRoadSiteCommandService _commands;
    private readonly IRoadSiteQueryService _queries;

    public ProjectsController(IRoadSiteCommandService commands, IRoadSiteQueryService queries)
    {
        _commands = commands;
        _queries = queries;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjects(CancellationToken cancellationToken)
    {
        var projects = await _queries.Handle(new GetAllRoadProjectsQuery(), cancellationToken);
        return Ok(RoadProjectResourceFromEntityAssembler.ToResourcesFromEntities(projects));
    }

    [HttpGet("{projectId}")]
    public async Task<IActionResult> GetProject(string projectId, CancellationToken cancellationToken)
    {
        var project = await _queries.Handle(new GetRoadProjectByIdQuery(projectId), cancellationToken);
        if (project == null)
        {
            return NotFound(new { message = "Road project was not found." });
        }

        return Ok(RoadProjectResourceFromEntityAssembler.ToResourceFromEntity(project));
    }

    [HttpPost]
    public async Task<IActionResult> RegisterProject(
        [FromBody] RegisterRoadProjectResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = RegisterRoadProjectCommandFromResourceAssembler.ToCommandFromResource(resource);
            var project = await _commands.Handle(command, cancellationToken);
            var body = RoadProjectResourceFromEntityAssembler.ToResourceFromEntity(project);
            return CreatedAtAction(nameof(GetProject), new { projectId = project.Id }, body);
        }
        catch (ProjectRuleException exception)
        {
            return MapRuleException(exception);
        }
    }

    [HttpPost("{projectId}/sections")]
    public async Task<IActionResult> ConfigureSection(
        string projectId,
        [FromBody] ConfigureRoadSectionResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = ConfigureRoadSectionCommandFromResourceAssembler.ToCommandFromResource(projectId, resource);
            var section = await _commands.Handle(command, cancellationToken);
            return Created(
                $"/api/projects/{projectId}/sections/{section.Id}",
                RoadProjectResourceFromEntityAssembler.ToSectionResource(section));
        }
        catch (ProjectRuleException exception)
        {
            return MapRuleException(exception);
        }
    }

    [HttpPut("{projectId}/sections/{sectionId}")]
    public async Task<IActionResult> UpdateSection(
        string projectId,
        string sectionId,
        [FromBody] ConfigureRoadSectionResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = ConfigureRoadSectionCommandFromResourceAssembler.ToUpdateCommandFromResource(
                projectId,
                sectionId,
                resource);
            var section = await _commands.Handle(command, cancellationToken);
            return Ok(RoadProjectResourceFromEntityAssembler.ToSectionResource(section));
        }
        catch (ProjectRuleException exception)
        {
            return MapRuleException(exception);
        }
    }

    [HttpDelete("{projectId}/sections/{sectionId}")]
    public async Task<IActionResult> DeleteSection(
        string projectId,
        string sectionId,
        CancellationToken cancellationToken)
    {
        try
        {
            await _commands.Handle(new DeleteRoadSectionCommand(projectId, sectionId), cancellationToken);
            return NoContent();
        }
        catch (ProjectRuleException exception)
        {
            return MapRuleException(exception);
        }
    }

    private IActionResult MapRuleException(ProjectRuleException exception)
    {
        if (exception.NotFound)
        {
            return NotFound(new { message = exception.Message });
        }

        return BadRequest(new { message = exception.Message });
    }
}
