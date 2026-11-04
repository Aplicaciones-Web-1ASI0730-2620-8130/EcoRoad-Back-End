using EcoRoad.Monitoring.Application.CommandServices;
using EcoRoad.Monitoring.Domain.Model;
using EcoRoad.Monitoring.Domain.Model.Aggregates;
using EcoRoad.Monitoring.Domain.Model.Commands;
using EcoRoad.Monitoring.Domain.Model.ValueObjects;
using EcoRoad.Monitoring.Domain.Repositories;
using EcoRoad.Shared.Domain.Model.Exceptions;
using EcoRoad.Shared.Domain.Repositories;

namespace EcoRoad.Monitoring.Application.Internal.CommandServices;

/// <summary>
/// Records automatic and manual measurements, classifying each one against its threshold profile.
/// </summary>
public class EnvironmentalMeasurementCommandService : IEnvironmentalMeasurementCommandService
{
    private readonly IEnvironmentalMeasurementRepository _measurements;
    private readonly IThresholdProfileRepository _profiles;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EnvironmentalMeasurementCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvironmentalMeasurementCommandService" /> class.
    /// </summary>
    /// <param name="measurements">Repository of measurements.</param>
    /// <param name="profiles">Repository of threshold profiles.</param>
    /// <param name="unitOfWork">Unit of work that persists the changes.</param>
    /// <param name="logger">Logger used to trace the handled commands.</param>
    public EnvironmentalMeasurementCommandService(
        IEnvironmentalMeasurementRepository measurements,
        IThresholdProfileRepository profiles,
        IUnitOfWork unitOfWork,
        ILogger<EnvironmentalMeasurementCommandService> logger)
    {
        _measurements = measurements;
        _profiles = profiles;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<EnvironmentalMeasurement> Handle(
        CaptureTelemetryReadingCommand command,
        CancellationToken cancellationToken = default)
    {
        var profile = await FindProfileAsync(command.ParameterType, cancellationToken);
        var now = DateTime.UtcNow;
        var measurement = EnvironmentalMeasurement.CaptureEnvironmentalReading(
            command.ProjectId,
            command.SectionId,
            command.MonitoringPointId,
            command.SensorId,
            command.Value,
            command.RecordedAt?.ToUniversalTime() ?? now,
            profile,
            now);

        return await SaveAsync(measurement, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<EnvironmentalMeasurement> Handle(
        RecordManualMeasurementCommand command,
        CancellationToken cancellationToken = default)
    {
        var profile = await FindProfileAsync(command.ParameterType, cancellationToken);
        var now = DateTime.UtcNow;
        var measurement = EnvironmentalMeasurement.RecordManualMeasurement(
            command.ProjectId,
            command.SectionId,
            command.OperatorId,
            command.Value,
            command.RecordedAt?.ToUniversalTime() ?? now,
            command.Notes,
            profile,
            now);

        return await SaveAsync(measurement, cancellationToken);
    }

    /// <summary>
    /// Loads the threshold profile of a parameter; measurements cannot be classified without one.
    /// </summary>
    /// <param name="parameterType">The environmental parameter.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The threshold profile.</returns>
    private async Task<ThresholdProfile> FindProfileAsync(ParameterType parameterType, CancellationToken cancellationToken)
    {
        return await _profiles.FindByParameterTypeAsync(parameterType, cancellationToken)
            ?? throw new MonitoringRuleException(
                "PROFILE_NOT_CONFIGURED",
                "Configure the threshold profile of the parameter before recording measurements.",
                DomainErrorKind.Conflict);
    }

    /// <summary>
    /// Persists a classified measurement.
    /// </summary>
    /// <param name="measurement">The measurement.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The same measurement.</returns>
    private async Task<EnvironmentalMeasurement> SaveAsync(
        EnvironmentalMeasurement measurement,
        CancellationToken cancellationToken)
    {
        await _measurements.AddAsync(measurement, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation(
            "Measurement {MeasurementId} of {Parameter} recorded as {Status} in section {SectionId}.",
            measurement.Id,
            measurement.ParameterType,
            measurement.Status,
            measurement.SectionId);
        return measurement;
    }
}
