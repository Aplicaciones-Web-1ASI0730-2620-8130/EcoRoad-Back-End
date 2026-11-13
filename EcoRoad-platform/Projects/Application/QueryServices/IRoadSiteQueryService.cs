using EcoRoad.Projects.Domain.Model.Aggregates;
using EcoRoad.Projects.Domain.Model.Queries;

namespace EcoRoad.Projects.Application.QueryServices;

public interface IRoadSiteQueryService
{
    Task<IReadOnlyList<RoadProject>> Handle(GetAllRoadProjectsQuery query, CancellationToken cancellationToken = default);

    Task<RoadProject?> Handle(GetRoadProjectByIdQuery query, CancellationToken cancellationToken = default);
}
