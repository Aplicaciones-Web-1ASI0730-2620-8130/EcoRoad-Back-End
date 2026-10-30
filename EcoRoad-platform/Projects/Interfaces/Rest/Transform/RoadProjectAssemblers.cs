using EcoRoad.Projects.Domain.Model.Aggregates;
using EcoRoad.Projects.Domain.Model.Commands;
using EcoRoad.Projects.Interfaces.Rest.Resources;

namespace EcoRoad.Projects.Interfaces.Rest.Transform;

public static class RoadProjectResourceFromEntityAssembler
{
    public static RoadProjectResource ToResourceFromEntity(RoadProject project)
    {
        return new RoadProjectResource(
            project.Id,
            project.CompanyId,
            project.Name,
            project.Code,
            project.Status,
            project.CreatedAt,
            project.Sections.Select(ToSectionResource).ToList());
    }

    public static IReadOnlyList<RoadProjectResource> ToResourcesFromEntities(IEnumerable<RoadProject> projects)
    {
        return projects.Select(ToResourceFromEntity).ToList();
    }

    public static RoadSectionResource ToSectionResource(RoadSection section)
    {
        return new RoadSectionResource(
            section.Id,
            section.Name,
            section.StartKilometer,
            section.EndKilometer,
            section.WorkFronts.Select(ToWorkFrontResource).ToList());
    }

    private static WorkFrontResource ToWorkFrontResource(WorkFront workFront)
    {
        return new WorkFrontResource(workFront.Id, workFront.Name, workFront.Status);
    }
}

public static class RegisterRoadProjectCommandFromResourceAssembler
{
    public static RegisterRoadProjectCommand ToCommandFromResource(RegisterRoadProjectResource resource)
    {
        return new RegisterRoadProjectCommand(
            resource.CompanyId,
            resource.Name,
            resource.Code,
            resource.Status,
            resource.Sections?.Select(ToSectionRegistration).ToList());
    }

    private static RoadSectionRegistration ToSectionRegistration(RegisterRoadSectionResource resource)
    {
        return new RoadSectionRegistration(
            resource.Name,
            resource.StartKilometer,
            resource.EndKilometer,
            resource.WorkFronts?.Select(front => new WorkFrontRegistration(front.Name, front.Status)).ToList());
    }
}

public static class ConfigureRoadSectionCommandFromResourceAssembler
{
    public static ConfigureRoadSectionCommand ToCommandFromResource(
        string projectId,
        ConfigureRoadSectionResource resource)
    {
        return new ConfigureRoadSectionCommand(
            projectId,
            resource.Name,
            resource.StartKilometer,
            resource.EndKilometer);
    }

    public static UpdateRoadSectionCommand ToUpdateCommandFromResource(
        string projectId,
        string sectionId,
        ConfigureRoadSectionResource resource)
    {
        return new UpdateRoadSectionCommand(
            projectId,
            sectionId,
            resource.Name,
            resource.StartKilometer,
            resource.EndKilometer);
    }
}
