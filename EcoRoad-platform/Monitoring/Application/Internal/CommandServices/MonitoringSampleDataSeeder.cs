using EcoRoad.Monitoring.Domain.Model.Aggregates;
using EcoRoad.Monitoring.Domain.Model.ValueObjects;
using EcoRoad.Monitoring.Domain.Repositories;
using EcoRoad.Shared.Domain.Repositories;

namespace EcoRoad.Monitoring.Application.Internal.CommandServices;

/// <summary>
/// Seeds demo threshold profiles and measurements used by the frontend during local development.
/// </summary>
/// <remarks>
/// The thresholds are illustrative values for the simulation and are not an official regulatory profile.
/// </remarks>
public class MonitoringSampleDataSeeder
{
    private const string LimaCanta = "prj-lima-canta";

    private readonly IThresholdProfileRepository _profiles;
    private readonly IEnvironmentalMeasurementRepository _measurements;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<MonitoringSampleDataSeeder> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MonitoringSampleDataSeeder" /> class.
    /// </summary>
    /// <param name="profiles">Repository of threshold profiles.</param>
    /// <param name="measurements">Repository of measurements.</param>
    /// <param name="unitOfWork">Unit of work that persists the changes.</param>
    /// <param name="logger">Logger used to trace the seeding.</param>
    public MonitoringSampleDataSeeder(
        IThresholdProfileRepository profiles,
        IEnvironmentalMeasurementRepository measurements,
        IUnitOfWork unitOfWork,
        ILogger<MonitoringSampleDataSeeder> logger)
    {
        _profiles = profiles;
        _measurements = measurements;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Creates the demo profiles and measurements when no profile exists.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    public async Task SeedIfEmptyAsync(CancellationToken cancellationToken = default)
    {
        if ((await _profiles.ListAsync(cancellationToken)).Count > 0)
        {
            return;
        }

        var profiles = new Dictionary<ParameterType, ThresholdProfile>
        {
            [ParameterType.AirQuality] = new("Demo PM10 profile", ParameterType.AirQuality, ThresholdLimits.ForMaximum(100, 150)),
            [ParameterType.NoiseLevel] = new("Demo noise profile", ParameterType.NoiseLevel, ThresholdLimits.ForMaximum(70, 80)),
            [ParameterType.WaterQuality] = new("Demo pH profile", ParameterType.WaterQuality, ThresholdLimits.ForRange(6.5, 6.8, 8.2, 8.5)),
            [ParameterType.Vibration] = new("Demo vibration profile", ParameterType.Vibration, ThresholdLimits.ForMaximum(4, 5))
        };
        foreach (var profile in profiles.Values)
        {
            await _profiles.AddAsync(profile, cancellationToken);
        }

        var now = DateTime.UtcNow;
        var telemetry = new (string Project, string Section, ParameterType Parameter, double Value, int MinutesAgo)[]
        {
            (LimaCanta, "lim-01", ParameterType.AirQuality, 42, 4),
            (LimaCanta, "lim-01", ParameterType.NoiseLevel, 62, 7),
            (LimaCanta, "lim-02", ParameterType.NoiseLevel, 72, 3),
            (LimaCanta, "lim-03", ParameterType.AirQuality, 185, 2),
            (LimaCanta, "lim-03", ParameterType.AirQuality, 121, 62),
            (LimaCanta, "lim-03", ParameterType.AirQuality, 68, 1440),
            (LimaCanta, "lim-03", ParameterType.AirQuality, 93, 2880),
            (LimaCanta, "lim-03", ParameterType.AirQuality, 74, 4320),
            (LimaCanta, "lim-03", ParameterType.AirQuality, 57, 5760),
            (LimaCanta, "lim-03", ParameterType.AirQuality, 112, 7200),
            (LimaCanta, "lim-03", ParameterType.AirQuality, 49, 8640),
            ("prj-arequipa-norte", "aqp-01", ParameterType.AirQuality, 162, 5)
        };
        foreach (var (project, section, parameter, value, minutesAgo) in telemetry)
        {
            await _measurements.AddAsync(
                EnvironmentalMeasurement.CaptureEnvironmentalReading(
                    project,
                    section,
                    $"pt-{section}",
                    $"sen-{section}-{parameter.Metric().ToLowerInvariant()}",
                    value,
                    now.AddMinutes(-minutesAgo),
                    profiles[parameter],
                    now),
                cancellationToken);
        }

        var manual = new (string Project, string Section, ParameterType Parameter, double Value, int MinutesAgo)[]
        {
            (LimaCanta, "lim-02", ParameterType.Vibration, 2.1, 8),
            (LimaCanta, "lim-04", ParameterType.WaterQuality, 6.8, 12),
            ("prj-cusco-anta", "cus-01", ParameterType.WaterQuality, 8.3, 6)
        };
        foreach (var (project, section, parameter, value, minutesAgo) in manual)
        {
            await _measurements.AddAsync(
                EnvironmentalMeasurement.RecordManualMeasurement(
                    project,
                    section,
                    "usr-ana",
                    value,
                    now.AddMinutes(-minutesAgo),
                    "Demo field measurement.",
                    profiles[parameter],
                    now),
                cancellationToken);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);
        _logger.LogInformation("Environmental monitoring demo data seeded.");
    }
}
