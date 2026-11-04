using EcoRoad.Assets.Domain.Model.Aggregates;
using EcoRoad.Assets.Domain.Model.Commands;
using EcoRoad.Assets.Interfaces.Rest.Resources;

namespace EcoRoad.Assets.Interfaces.Rest.Transform;

/// <summary>
/// Converts <see cref="MonitoringPoint" /> aggregates into resources.
/// </summary>
public static class MonitoringPointResourceFromEntityAssembler
{
    /// <summary>
    /// Converts a monitoring point into its resource.
    /// </summary>
    /// <param name="point">The point to convert.</param>
    /// <returns>The point resource.</returns>
    public static MonitoringPointResource ToResourceFromEntity(MonitoringPoint point)
    {
        return new MonitoringPointResource(
            point.Id,
            point.Code,
            point.ProjectId,
            point.SectionId,
            point.Name,
            point.KilometerPost,
            point.Latitude,
            point.Longitude,
            point.Status,
            point.CreatedAt);
    }

    /// <summary>
    /// Converts a list of monitoring points into resources.
    /// </summary>
    /// <param name="points">The points to convert.</param>
    /// <returns>The point resources.</returns>
    public static IReadOnlyList<MonitoringPointResource> ToResourcesFromEntities(IEnumerable<MonitoringPoint> points)
    {
        return points.Select(ToResourceFromEntity).ToList();
    }
}

/// <summary>
/// Converts monitoring point resources into commands.
/// </summary>
public static class MonitoringPointCommandFromResourceAssembler
{
    /// <summary>
    /// Builds the registration command.
    /// </summary>
    /// <param name="resource">The request body.</param>
    /// <returns>The command.</returns>
    public static RegisterMonitoringPointCommand ToRegisterCommand(SaveMonitoringPointResource resource)
    {
        return new RegisterMonitoringPointCommand(
            resource.ProjectId,
            resource.SectionId,
            resource.Name,
            resource.KilometerPost,
            resource.Latitude,
            resource.Longitude);
    }

    /// <summary>
    /// Builds the update command.
    /// </summary>
    /// <param name="pointId">Identifier of the point.</param>
    /// <param name="resource">The request body.</param>
    /// <returns>The command.</returns>
    public static UpdateMonitoringPointCommand ToUpdateCommand(string pointId, SaveMonitoringPointResource resource)
    {
        return new UpdateMonitoringPointCommand(
            pointId,
            resource.ProjectId,
            resource.SectionId,
            resource.Name,
            resource.KilometerPost,
            resource.Latitude,
            resource.Longitude);
    }
}
