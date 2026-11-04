namespace EcoRoad.Assets.Domain.Model.ValueObjects;

/// <summary>
/// Environmental variable captured by a sensor of the EcoRoad hardware as a service fleet.
/// </summary>
public enum SensorType
{
    /// <summary>Particulate matter PM10 station, measured in micrograms per cubic meter.</summary>
    Pm10 = 1,

    /// <summary>Environmental noise meter (LAeq), measured in decibels A.</summary>
    Noise = 2,

    /// <summary>Water quality probe, measured in pH units.</summary>
    Water = 3,

    /// <summary>Field accelerometer for ground vibration, measured in millimeters per second.</summary>
    Vibration = 4
}
