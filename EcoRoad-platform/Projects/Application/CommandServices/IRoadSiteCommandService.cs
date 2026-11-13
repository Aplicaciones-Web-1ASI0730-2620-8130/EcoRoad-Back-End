using EcoRoad.Projects.Domain.Model.Aggregates;
using EcoRoad.Projects.Domain.Model.Commands;

namespace EcoRoad.Projects.Application.CommandServices;

public interface IRoadSiteCommandService
{
    Task<RoadProject> Handle(RegisterRoadProjectCommand command, CancellationToken cancellationToken = default);

    Task<RoadSection> Handle(ConfigureRoadSectionCommand command, CancellationToken cancellationToken = default);

    Task<WorkFront> Handle(RegisterWorkFrontCommand command, CancellationToken cancellationToken = default);

    Task<RoadSection> Handle(UpdateRoadSectionCommand command, CancellationToken cancellationToken = default);

    Task Handle(DeleteRoadSectionCommand command, CancellationToken cancellationToken = default);
}
