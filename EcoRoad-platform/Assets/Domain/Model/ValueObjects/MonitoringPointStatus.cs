namespace EcoRoad.Assets.Domain.Model.ValueObjects;

/// <summary>
/// Operational status of a monitoring point.
/// </summary>
public enum MonitoringPointStatus
{
    /// <summary>The point is geolocated and can receive sensors.</summary>
    Enabled = 1,

    /// <summary>The point is out of service and cannot receive sensors.</summary>
    Disabled = 2
}
