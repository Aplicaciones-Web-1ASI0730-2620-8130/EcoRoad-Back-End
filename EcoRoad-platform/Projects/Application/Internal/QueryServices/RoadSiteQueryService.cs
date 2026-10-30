using EcoRoad.Projects.Application.QueryServices;
using EcoRoad.Projects.Domain.Model.Aggregates;
using EcoRoad.Projects.Domain.Model.Queries;
using EcoRoad.Projects.Domain.Repositories;

namespace EcoRoad.Projects.Application.Internal.QueryServices;

public class RoadSiteQueryService : IRoadSiteQueryService
{
    private readonly IRoadProjectRepository _projects;

    public RoadSiteQueryService(IRoadProjectRepository projects)
    {
        _projects = projects;
    }

    public async Task<IReadOnlyList<RoadProject>> Handle(
        GetAllRoadProjectsQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _projects.GetAllAsync(cancellationToken);
    }

    public async Task<RoadProject?> Handle(
        GetRoadProjectByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _projects.GetByIdWithSectionsAsync(query.ProjectId, cancellationToken);
    }
}
