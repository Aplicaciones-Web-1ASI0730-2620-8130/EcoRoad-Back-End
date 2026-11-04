namespace EcoRoad.Assets.Domain.Model.Commands;

/// <summary>
/// Moves a monitoring point or changes its data. Only allowed while no sensor is installed.
/// </summary>
/// <param name="MonitoringPointId">Identifier of the point.</param>
/// <param name="ProjectId">Identifier of the road project.</param>
/// <param name="SectionId">Identifier of the road section.</param>
/// <param name="Name">Descriptive name of the point.</param>
/// <param name="KilometerPost">Kilometer post, for example <c>PK 05+000</c>.</param>
/// <param name="Latitude">Latitude in decimal degrees.</param>
/// <param name="Longitude">Longitude in decimal degrees.</param>
public record UpdateMonitoringPointCommand(
    string MonitoringPointId,
    string ProjectId,
    string SectionId,
    string Name,
    string KilometerPost,
    double Latitude,
    double Longitude);
