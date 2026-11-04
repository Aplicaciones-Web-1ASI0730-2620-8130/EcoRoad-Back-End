using EcoRoad.Monitoring.Application.QueryServices;
using EcoRoad.Monitoring.Domain.Model;
using EcoRoad.Monitoring.Domain.Model.Aggregates;
using EcoRoad.Monitoring.Domain.Model.Queries;
using EcoRoad.Monitoring.Domain.Repositories;
using EcoRoad.Monitoring.Domain.Services;

namespace EcoRoad.Monitoring.Application.Internal.QueryServices;

/// <summary>
/// Answers the queries about measurements, dashboards and histories.
/// </summary>
public class EnvironmentalMeasurementQueryService : IEnvironmentalMeasurementQueryService
{
    /// <summary>Window of recent measurements considered by the dashboard.</summary>
    public static readonly TimeSpan DashboardWindow = TimeSpan.FromDays(30);

    private readonly IEnvironmentalMeasurementRepository _measurements;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvironmentalMeasurementQueryService" /> class.
    /// </summary>
    /// <param name="measurements">Repository of measurements.</param>
    public EnvironmentalMeasurementQueryService(IEnvironmentalMeasurementRepository measurements)
    {
        _measurements = measurements;
    }

    /// <inheritdoc />
    public async Task<EnvironmentalMeasurement?> Handle(
        GetMeasurementByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _measurements.FindByIdAsync(query.MeasurementId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<MonitoringDashboard> Handle(
        GetMonitoringDashboardQuery query,
        CancellationToken cancellationToken = default)
    {
        var since = DateTime.UtcNow.Subtract(DashboardWindow);
        var recent = await _measurements.ListRecentByProjectIdAsync(query.ProjectId, since, cancellationToken);
        return MonitoringDashboardBuilder.Build(query.ProjectId, recent);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<EnvironmentalMeasurement>> Handle(
        GetMeasurementHistoryQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.From.HasValue && query.To.HasValue && query.From.Value > query.To.Value)
        {
            throw new MonitoringRuleException(
                "INVALID_HISTORY_FILTERS",
                "The start date must be earlier than or equal to the end date.");
        }

        var from = query.From?.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var to = query.To?.AddDays(1).ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        return await _measurements.ListHistoryAsync(
            query.ProjectId,
            query.SectionId,
            query.ParameterType,
            from,
            to,
            cancellationToken);
    }
}
