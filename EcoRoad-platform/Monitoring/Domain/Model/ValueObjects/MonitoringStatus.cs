namespace EcoRoad.Monitoring.Domain.Model.ValueObjects;

/// <summary>
/// Classification of a measurement against its threshold profile. Higher values are worse.
/// </summary>
public enum MonitoringStatus
{
    /// <summary>The value is within the optimal band.</summary>
    Optimal = 1,

    /// <summary>The value left the optimal band and must be watched.</summary>
    Observation = 2,

    /// <summary>The value exceeds the critical limit.</summary>
    Critical = 3
}
