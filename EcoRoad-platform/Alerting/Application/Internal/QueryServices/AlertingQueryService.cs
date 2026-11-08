using EcoRoad.Alerting.Application.QueryServices;
using EcoRoad.Alerting.Domain.Model.Aggregates;
using EcoRoad.Alerting.Domain.Model.Queries;
using EcoRoad.Alerting.Domain.Repositories;

namespace EcoRoad.Alerting.Application.Internal.QueryServices;

/// <summary>
///     Reads alerts and deviation assessments.
/// </summary>
public class AlertingQueryService : IAlertingQueryService
{
    private readonly IAlertRepository _alerts;
    private readonly IDeviationAssessmentRepository _assessments;

    public AlertingQueryService(
        IAlertRepository alerts,
        IDeviationAssessmentRepository assessments)
    {
        _alerts = alerts;
        _assessments = assessments;
    }

    public async Task<ActiveAlertView?> Handle(
        GetAlertByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var alert = await _alerts.FindByIdAsync(query.AlertId, cancellationToken);
        return alert == null ? null : ToView(alert);
    }

    public async Task<IReadOnlyList<ActiveAlertView>> Handle(
        GetAlertsQuery query,
        CancellationToken cancellationToken = default)
    {
        var alerts = await _alerts.ListAsync(query.MonitoringPointId, query.Status, cancellationToken);
        return alerts.Select(ToView).ToList();
    }

    public async Task<IReadOnlyList<ActiveAlertView>> Handle(
        GetActiveAlertsQuery query,
        CancellationToken cancellationToken = default)
    {
        var alerts = await _alerts.ListActiveAsync(cancellationToken);
        return alerts.Select(ToView).ToList();
    }

    public async Task<DeviationAssessmentView?> Handle(
        GetDeviationAssessmentByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var assessment = await _assessments.FindByIdAsync(query.AssessmentId, cancellationToken);
        return assessment == null ? null : ToView(assessment);
    }

    public async Task<IReadOnlyList<DeviationAssessmentView>> Handle(
        GetDeviationAssessmentsQuery query,
        CancellationToken cancellationToken = default)
    {
        var assessments = await _assessments.ListAsync(query.MeasurementId, cancellationToken);
        return assessments.Select(ToView).ToList();
    }

    private static ActiveAlertView ToView(Alert alert)
    {
        return new ActiveAlertView(
            alert.Id,
            alert.AssessmentId,
            alert.MonitoringPointId,
            alert.Severity,
            alert.Status,
            alert.Message,
            alert.RaisedAt,
            alert.AcknowledgedAt,
            alert.AcknowledgedBy);
    }

    private static DeviationAssessmentView ToView(DeviationAssessment assessment)
    {
        return new DeviationAssessmentView(
            assessment.Id,
            assessment.MeasurementId,
            assessment.MeasuredValue,
            assessment.ThresholdValue,
            assessment.IsCritical,
            assessment.AssessedAt);
    }
}
