using EcoRoad.Assets.Application.CommandServices;
using EcoRoad.Assets.Domain.Model;
using EcoRoad.Assets.Domain.Model.Aggregates;
using EcoRoad.Assets.Domain.Model.Commands;
using EcoRoad.Assets.Domain.Repositories;
using EcoRoad.Shared.Domain.Model.Exceptions;
using EcoRoad.Shared.Domain.Repositories;

namespace EcoRoad.Assets.Application.Internal.CommandServices;

/// <summary>
/// Handles the commands that register and maintain monitoring points.
/// </summary>
public class MonitoringPointCommandService : IMonitoringPointCommandService
{
    private readonly IMonitoringPointRepository _points;
    private readonly ISensorRepository _sensors;
    private readonly ISensorAssignmentRepository _assignments;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<MonitoringPointCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MonitoringPointCommandService" /> class.
    /// </summary>
    /// <param name="points">Repository of monitoring points.</param>
    /// <param name="sensors">Repository of sensors.</param>
    /// <param name="assignments">Repository of the assignment history.</param>
    /// <param name="unitOfWork">Unit of work that persists the changes.</param>
    /// <param name="logger">Logger used to trace the handled commands.</param>
    public MonitoringPointCommandService(
        IMonitoringPointRepository points,
        ISensorRepository sensors,
        ISensorAssignmentRepository assignments,
        IUnitOfWork unitOfWork,
        ILogger<MonitoringPointCommandService> logger)
    {
        _points = points;
        _sensors = sensors;
        _assignments = assignments;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<MonitoringPoint> Handle(
        RegisterMonitoringPointCommand command,
        CancellationToken cancellationToken = default)
    {
        var point = new MonitoringPoint(
            command.ProjectId,
            command.SectionId,
            command.Name,
            command.KilometerPost,
            command.Latitude,
            command.Longitude);

        await EnsureNameIsFreeAsync(point, cancellationToken);
        await _points.AddAsync(point, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Monitoring point {PointId} registered in section {SectionId}.", point.Id, point.SectionId);
        return point;
    }

    /// <inheritdoc />
    public async Task<MonitoringPoint> Handle(
        UpdateMonitoringPointCommand command,
        CancellationToken cancellationToken = default)
    {
        var point = await FindPointAsync(command.MonitoringPointId, cancellationToken);
        await EnsureHasNoSensorsAsync(point, cancellationToken);

        point.Relocate(
            command.ProjectId,
            command.SectionId,
            command.Name,
            command.KilometerPost,
            command.Latitude,
            command.Longitude);

        await EnsureNameIsFreeAsync(point, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Monitoring point {PointId} updated.", point.Id);
        return point;
    }

    /// <inheritdoc />
    public async Task Handle(DeleteMonitoringPointCommand command, CancellationToken cancellationToken = default)
    {
        var point = await FindPointAsync(command.MonitoringPointId, cancellationToken);
        await EnsureHasNoSensorsAsync(point, cancellationToken);

        // Measurements and audits reference the point, so a point with history is only disabled.
        if (await _assignments.ExistsByMonitoringPointIdAsync(point.Id, cancellationToken))
        {
            throw new AssetsRuleException(
                "POINT_HAS_HISTORY",
                "The monitoring point has deployment history. Disable it instead of deleting it.",
                DomainErrorKind.Conflict);
        }

        _points.Remove(point);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Monitoring point {PointId} deleted.", point.Id);
    }

    /// <inheritdoc />
    public async Task<MonitoringPoint> Handle(
        EnableMonitoringPointCommand command,
        CancellationToken cancellationToken = default)
    {
        var point = await FindPointAsync(command.MonitoringPointId, cancellationToken);
        point.Enable();
        await _unitOfWork.CompleteAsync(cancellationToken);
        return point;
    }

    /// <inheritdoc />
    public async Task<MonitoringPoint> Handle(
        DisableMonitoringPointCommand command,
        CancellationToken cancellationToken = default)
    {
        var point = await FindPointAsync(command.MonitoringPointId, cancellationToken);
        await EnsureHasNoSensorsAsync(point, cancellationToken);
        point.Disable();
        await _unitOfWork.CompleteAsync(cancellationToken);
        return point;
    }

    /// <summary>
    /// Loads a monitoring point or fails with <c>POINT_NOT_FOUND</c>.
    /// </summary>
    /// <param name="pointId">Identifier of the point.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The monitoring point.</returns>
    private async Task<MonitoringPoint> FindPointAsync(string pointId, CancellationToken cancellationToken)
    {
        return await _points.FindByIdAsync(pointId, cancellationToken)
            ?? throw new AssetsRuleException("POINT_NOT_FOUND", "The monitoring point was not found.", DomainErrorKind.NotFound);
    }

    /// <summary>
    /// Ensures no sensor is installed at the point, so it can be moved, disabled or deleted.
    /// </summary>
    /// <param name="point">The monitoring point.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    private async Task EnsureHasNoSensorsAsync(MonitoringPoint point, CancellationToken cancellationToken)
    {
        if (await _sensors.CountByMonitoringPointIdAsync(point.Id, cancellationToken) > 0)
        {
            throw new AssetsRuleException(
                "POINT_IN_USE",
                "Release the sensors of the monitoring point first.",
                DomainErrorKind.Conflict);
        }
    }

    /// <summary>
    /// Ensures the name of the point is unique inside its road section.
    /// </summary>
    /// <param name="point">The monitoring point.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    private async Task EnsureNameIsFreeAsync(MonitoringPoint point, CancellationToken cancellationToken)
    {
        if (await _points.ExistsByNameInSectionAsync(point.SectionId, point.Name, point.Id, cancellationToken))
        {
            throw new AssetsRuleException(
                "DUPLICATE_POINT",
                "Another monitoring point of the section already uses this name.",
                DomainErrorKind.Conflict);
        }
    }
}
