using EcoRoad.Monitoring.Domain.Model.Aggregates;
using EcoRoad.Monitoring.Domain.Model.Commands;

namespace EcoRoad.Monitoring.Application.CommandServices;

/// <summary>
/// Application service that records environmental measurements.
/// </summary>
public interface IEnvironmentalMeasurementCommandService
{
    /// <summary>Captures an automatic sensor reading.</summary>
    /// <param name="command">The telemetry command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The classified measurement.</returns>
    Task<EnvironmentalMeasurement> Handle(CaptureTelemetryReadingCommand command, CancellationToken cancellationToken = default);

    /// <summary>Records a manual field measurement.</summary>
    /// <param name="command">The manual measurement command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The classified measurement.</returns>
    Task<EnvironmentalMeasurement> Handle(RecordManualMeasurementCommand command, CancellationToken cancellationToken = default);
}
