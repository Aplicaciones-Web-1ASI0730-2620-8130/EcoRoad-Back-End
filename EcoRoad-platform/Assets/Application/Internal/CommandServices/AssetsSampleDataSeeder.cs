using EcoRoad.Assets.Domain.Model.Aggregates;
using EcoRoad.Assets.Domain.Model.ValueObjects;
using EcoRoad.Assets.Domain.Repositories;
using EcoRoad.Shared.Domain.Repositories;

namespace EcoRoad.Assets.Application.Internal.CommandServices;

/// <summary>
/// Seeds the demo monitoring points and sensors used by the frontend during local development.
/// </summary>
/// <remarks>
/// Project and section identifiers match the demo road projects of the frontend fixtures.
/// Sensors go through the real deployment lifecycle, so the demo data respects every rule.
/// </remarks>
public class AssetsSampleDataSeeder
{
    private readonly IMonitoringPointRepository _points;
    private readonly ISensorRepository _sensors;
    private readonly ISensorAssignmentRepository _assignments;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AssetsSampleDataSeeder> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssetsSampleDataSeeder" /> class.
    /// </summary>
    /// <param name="points">Repository of monitoring points.</param>
    /// <param name="sensors">Repository of sensors.</param>
    /// <param name="assignments">Repository of the assignment history.</param>
    /// <param name="unitOfWork">Unit of work that persists the changes.</param>
    /// <param name="logger">Logger used to trace the seeding.</param>
    public AssetsSampleDataSeeder(
        IMonitoringPointRepository points,
        ISensorRepository sensors,
        ISensorAssignmentRepository assignments,
        IUnitOfWork unitOfWork,
        ILogger<AssetsSampleDataSeeder> logger)
    {
        _points = points;
        _sensors = sensors;
        _assignments = assignments;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Creates the demo points and sensors when the inventory is empty.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    public async Task SeedIfEmptyAsync(CancellationToken cancellationToken = default)
    {
        if ((await _sensors.ListAsync(cancellationToken)).Count > 0)
        {
            return;
        }

        var now = DateTime.UtcNow;
        var calibrationValidUntil = now.AddDays(180);

        var trapiche = new MonitoringPoint("prj-lima-canta", "lim-01", "Estacion Trapiche", "05+000", -11.7182, -76.8503);
        var yangas = new MonitoringPoint("prj-lima-canta", "lim-02", "Frente Yangas", "45+100", -11.6628, -76.6468);
        var huamantanga = new MonitoringPoint("prj-lima-canta", "lim-03", "Estacion Huamantanga", "74+200", -11.5481, -76.5112);
        var accesoNorte = new MonitoringPoint("prj-arequipa-norte", "aqp-01", "Acceso Norte", "18+300", -16.2851, -71.5237);
        foreach (var point in new[] { trapiche, yangas, huamantanga, accesoNorte })
        {
            await _points.AddAsync(point, cancellationToken);
        }

        await DeployAsync(new Sensor("Estacion optica PM10", "ECO-PM10-001", SensorType.Pm10, "OPC-N3"), trapiche, calibrationValidUntil, true, now, cancellationToken);
        await DeployAsync(new Sensor("Sonometro LAeq", "ECO-NOISE-002", SensorType.Noise, "SL-4033"), yangas, calibrationValidUntil, true, now, cancellationToken);
        await DeployAsync(new Sensor("Muestreador de agua", "ECO-WATER-003", SensorType.Water, "PH-200"), accesoNorte, calibrationValidUntil, true, now, cancellationToken);
        await DeployAsync(new Sensor("Acelerometro de campo", "ECO-VIB-004", SensorType.Vibration, "VB-10"), huamantanga, null, false, now, cancellationToken);
        await _sensors.AddAsync(new Sensor("Sensor PM10 de reserva", "ECO-PM10-005", SensorType.Pm10, "OPC-N3"), cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);
        _logger.LogInformation("Asset demo data seeded.");
    }

    /// <summary>
    /// Registers a sensor and moves it through the deployment lifecycle.
    /// </summary>
    /// <param name="sensor">The sensor to deploy.</param>
    /// <param name="point">The destination monitoring point.</param>
    /// <param name="calibrationValidUntil">Calibration validity, or <c>null</c> to leave it uncalibrated.</param>
    /// <param name="activate">Whether the sensor must end active.</param>
    /// <param name="now">Current UTC date and time.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    private async Task DeployAsync(
        Sensor sensor,
        MonitoringPoint point,
        DateTime? calibrationValidUntil,
        bool activate,
        DateTime now,
        CancellationToken cancellationToken)
    {
        sensor.AssignTo(point);
        if (calibrationValidUntil.HasValue)
        {
            sensor.Calibrate(calibrationValidUntil.Value, now);
        }

        if (activate)
        {
            sensor.Activate(now);
        }

        await _sensors.AddAsync(sensor, cancellationToken);
        await _assignments.AddAsync(new SensorAssignment(sensor.Id, point.Id, now), cancellationToken);
    }
}
