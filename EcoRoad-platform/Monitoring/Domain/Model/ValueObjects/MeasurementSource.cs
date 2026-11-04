namespace EcoRoad.Monitoring.Domain.Model.ValueObjects;

/// <summary>
/// Origin of an environmental measurement.
/// </summary>
public enum MeasurementSource
{
    /// <summary>Captured automatically by an IoT sensor.</summary>
    AutomaticSensor = 1,

    /// <summary>Recorded manually by an operator on site.</summary>
    ManualEntry = 2
}
