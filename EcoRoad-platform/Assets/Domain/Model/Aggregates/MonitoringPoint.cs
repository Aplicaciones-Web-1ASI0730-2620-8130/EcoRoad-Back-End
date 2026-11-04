using EcoRoad.Assets.Domain.Model.ValueObjects;
using EcoRoad.Shared.Domain.Model.Entities;
using EcoRoad.Shared.Domain.Model.Exceptions;

namespace EcoRoad.Assets.Domain.Model.Aggregates;

/// <summary>
/// Geolocated place of a road section where sensors are installed.
/// </summary>
/// <remarks>
/// Projects and sections belong to the Project and Road Site Management bounded context;
/// this aggregate only references them by identifier.
/// </remarks>
public class MonitoringPoint : IAuditableEntity
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    private MonitoringPoint()
    {
    }

    /// <summary>
    /// Registers a new, enabled monitoring point.
    /// </summary>
    /// <param name="projectId">Identifier of the road project.</param>
    /// <param name="sectionId">Identifier of the road section.</param>
    /// <param name="name">Descriptive name of the point.</param>
    /// <param name="kilometerPost">Kilometer post, for example <c>PK 05+000</c>.</param>
    /// <param name="latitude">Latitude in decimal degrees.</param>
    /// <param name="longitude">Longitude in decimal degrees.</param>
    public MonitoringPoint(
        string projectId,
        string sectionId,
        string name,
        string kilometerPost,
        double latitude,
        double longitude)
    {
        Id = Guid.NewGuid().ToString();
        Code = AssetsRules.CodeFrom("PT", Id);
        Status = MonitoringPointStatus.Enabled;
        Relocate(projectId, sectionId, name, kilometerPost, latitude, longitude);
    }

    /// <summary>Gets the unique identifier of the point.</summary>
    public string Id { get; private set; } = string.Empty;

    /// <summary>Gets the short human readable code of the point.</summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the road project.</summary>
    public string ProjectId { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the road section.</summary>
    public string SectionId { get; private set; } = string.Empty;

    /// <summary>Gets the descriptive name of the point.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets the canonical kilometer post, for example <c>05+000</c>.</summary>
    public string KilometerPost { get; private set; } = string.Empty;

    /// <summary>Gets the latitude in decimal degrees.</summary>
    public double Latitude { get; private set; }

    /// <summary>Gets the longitude in decimal degrees.</summary>
    public double Longitude { get; private set; }

    /// <summary>Gets the operational status of the point.</summary>
    public MonitoringPointStatus Status { get; private set; }

    /// <inheritdoc />
    public DateTime? CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets a value indicating whether the point can receive sensors.
    /// </summary>
    public bool IsEnabled => Status == MonitoringPointStatus.Enabled;

    /// <summary>
    /// Moves the point or changes its descriptive data.
    /// </summary>
    /// <param name="projectId">Identifier of the road project.</param>
    /// <param name="sectionId">Identifier of the road section.</param>
    /// <param name="name">Descriptive name of the point.</param>
    /// <param name="kilometerPost">Kilometer post, for example <c>PK 05+000</c>.</param>
    /// <param name="latitude">Latitude in decimal degrees.</param>
    /// <param name="longitude">Longitude in decimal degrees.</param>
    public void Relocate(
        string projectId,
        string sectionId,
        string name,
        string kilometerPost,
        double latitude,
        double longitude)
    {
        ProjectId = AssetsRules.Require(projectId, "Project id", 36, "INVALID_POINT");
        SectionId = AssetsRules.Require(sectionId, "Section id", 36, "INVALID_POINT");
        Name = AssetsRules.Require(name, "Name", 100, "INVALID_POINT");
        KilometerPost = AssetsRules.RequireKilometerPost(kilometerPost);
        Geolocate(latitude, longitude);
    }

    /// <summary>
    /// Updates the coordinates of the point.
    /// </summary>
    /// <param name="latitude">Latitude in decimal degrees.</param>
    /// <param name="longitude">Longitude in decimal degrees.</param>
    public void Geolocate(double latitude, double longitude)
    {
        AssetsRules.RequireCoordinates(latitude, longitude);
        Latitude = latitude;
        Longitude = longitude;
    }

    /// <summary>
    /// Puts the point back into service.
    /// </summary>
    public void Enable()
    {
        if (IsEnabled)
        {
            throw new AssetsRuleException("POINT_ALREADY_ENABLED", "The monitoring point is already enabled.", DomainErrorKind.Conflict);
        }

        Status = MonitoringPointStatus.Enabled;
    }

    /// <summary>
    /// Takes the point out of service. The caller must verify the point has no sensors.
    /// </summary>
    public void Disable()
    {
        if (!IsEnabled)
        {
            throw new AssetsRuleException("POINT_ALREADY_DISABLED", "The monitoring point is already disabled.", DomainErrorKind.Conflict);
        }

        Status = MonitoringPointStatus.Disabled;
    }
}
