namespace EcoRoad.Monitoring.Domain.Model.ValueObjects;

/// <summary>
/// Environmental parameter monitored on the road works.
/// </summary>
public enum ParameterType
{
    /// <summary>Air quality measured as particulate matter PM10, in micrograms per cubic meter.</summary>
    AirQuality = 1,

    /// <summary>Environmental noise measured as LAeq, in decibels A.</summary>
    NoiseLevel = 2,

    /// <summary>Water quality measured as pH.</summary>
    WaterQuality = 3,

    /// <summary>Ground vibration measured as peak particle velocity, in millimeters per second.</summary>
    Vibration = 4
}
