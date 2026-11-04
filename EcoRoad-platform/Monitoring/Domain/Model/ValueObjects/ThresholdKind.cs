namespace EcoRoad.Monitoring.Domain.Model.ValueObjects;

/// <summary>
/// Shape of the limits of a threshold profile.
/// </summary>
public enum ThresholdKind
{
    /// <summary>Only an upper limit matters, for example PM10 or noise.</summary>
    Maximum = 1,

    /// <summary>The value must stay inside a band, for example pH.</summary>
    Range = 2
}
