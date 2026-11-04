namespace EcoRoad.Monitoring.Domain.Model.ValueObjects;

/// <summary>
/// Catalog of the metric, unit and physical limits of every <see cref="ParameterType" />.
/// </summary>
public static class ParameterTypeExtensions
{
    /// <summary>
    /// Gets the name of the metric measured for a parameter.
    /// </summary>
    /// <param name="parameter">The environmental parameter.</param>
    /// <returns>The metric, for example <c>PM10</c>.</returns>
    public static string Metric(this ParameterType parameter)
    {
        return parameter switch
        {
            ParameterType.AirQuality => "PM10",
            ParameterType.NoiseLevel => "LAeq",
            ParameterType.WaterQuality => "pH",
            ParameterType.Vibration => "PPV",
            _ => throw new ArgumentOutOfRangeException(nameof(parameter), parameter, "Unknown parameter.")
        };
    }

    /// <summary>
    /// Gets the unit used to record a parameter.
    /// </summary>
    /// <param name="parameter">The environmental parameter.</param>
    /// <returns>The unit, for example <c>ug/m3</c>.</returns>
    public static string Unit(this ParameterType parameter)
    {
        return parameter switch
        {
            ParameterType.AirQuality => "ug/m3",
            ParameterType.NoiseLevel => "dBA",
            ParameterType.WaterQuality => "pH",
            ParameterType.Vibration => "mm/s",
            _ => throw new ArgumentOutOfRangeException(nameof(parameter), parameter, "Unknown parameter.")
        };
    }

    /// <summary>
    /// Checks whether a value is physically possible for a parameter.
    /// </summary>
    /// <param name="parameter">The environmental parameter.</param>
    /// <param name="value">The measured value.</param>
    /// <returns><c>true</c> when the value can be a real measurement.</returns>
    public static bool IsPlausible(this ParameterType parameter, double value)
    {
        if (!double.IsFinite(value) || value < 0)
        {
            return false;
        }

        return parameter != ParameterType.WaterQuality || value <= 14;
    }
}
