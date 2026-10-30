using EcoRoad.Projects.Application.CommandServices;
using EcoRoad.Projects.Domain.Model;
using EcoRoad.Projects.Domain.Model.Aggregates;
using EcoRoad.Projects.Domain.Model.Commands;
using EcoRoad.Projects.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace EcoRoad.Projects.Application.Internal.CommandServices;

public class RoadSiteCommandService : IRoadSiteCommandService
{
    private readonly IRoadProjectRepository _projects;
    private readonly IRoadSectionRepository _sections;
    private readonly IWorkFrontRepository _workFronts;
    private readonly ILogger<RoadSiteCommandService> _logger;

    public RoadSiteCommandService(
        IRoadProjectRepository projects,
        IRoadSectionRepository sections,
        IWorkFrontRepository workFronts,
        ILogger<RoadSiteCommandService> logger)
    {
        _projects = projects;
        _sections = sections;
        _workFronts = workFronts;
        _logger = logger;
    }

    public async Task<RoadProject> Handle(
        RegisterRoadProjectCommand command,
        CancellationToken cancellationToken = default)
    {
        var project = new RoadProject(command.CompanyId, command.Name, command.Code, command.Status);

        foreach (var sectionRegistration in command.Sections ?? Array.Empty<RoadSectionRegistration>())
        {
            var section = new RoadSection(
                project.Id,
                sectionRegistration.Name,
                sectionRegistration.StartKilometer,
                sectionRegistration.EndKilometer);

            foreach (var workFrontRegistration in sectionRegistration.WorkFronts ?? Array.Empty<WorkFrontRegistration>())
            {
                section.WorkFronts.Add(new WorkFront(
                    section.Id,
                    workFrontRegistration.Name,
                    workFrontRegistration.Status));
            }

            project.Sections.Add(section);
        }

        await _projects.AddAsync(project, cancellationToken);
        _logger.LogInformation("Road project {ProjectId} registered.", project.Id);
        return project;
    }

    public async Task<RoadSection> Handle(
        ConfigureRoadSectionCommand command,
        CancellationToken cancellationToken = default)
    {
        var project = await _projects.GetByIdAsync(command.ProjectId, cancellationToken);
        if (project == null)
        {
            throw new ProjectRuleException("Road project was not found.");
        }

        var section = new RoadSection(
            project.Id,
            command.Name,
            command.StartKilometer,
            command.EndKilometer);

        await _sections.AddAsync(section, cancellationToken);
        _logger.LogInformation(
            "Road section {SectionId} configured for project {ProjectId}.",
            section.Id,
            project.Id);
        return section;
    }

    public async Task<WorkFront> Handle(
        RegisterWorkFrontCommand command,
        CancellationToken cancellationToken = default)
    {
        var section = await _sections.GetByIdAsync(command.SectionId, cancellationToken);
        if (section == null)
        {
            throw new ProjectRuleException("Road section was not found.");
        }

        var workFront = new WorkFront(section.Id, command.Name, command.Status);
        await _workFronts.AddAsync(workFront, cancellationToken);
        _logger.LogInformation(
            "Work front {WorkFrontId} registered for section {SectionId}.",
            workFront.Id,
            section.Id);
        return workFront;
    }
}
