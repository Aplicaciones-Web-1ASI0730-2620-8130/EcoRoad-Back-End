namespace EcoRoad.Assets.Domain.Model.ValueObjects;

/// <summary>
/// Deployment status of a sensor.
/// </summary>
public enum SensorStatus
{
    /// <summary>The sensor is in stock and not linked to any monitoring point.</summary>
    Available = 1,

    /// <summary>The sensor is installed at a monitoring point but does not report yet.</summary>
    Assigned = 2,

    /// <summary>The sensor is calibrated, activated and authorized to report measurements.</summary>
    Active = 3
}
