using EcoRoad.Alerting.Application.CommandServices;
using EcoRoad.Alerting.Domain.Model;
using EcoRoad.Alerting.Domain.Model.Aggregates;
using EcoRoad.Alerting.Domain.Model.Commands;
using EcoRoad.Alerting.Domain.Model.ValueObjects;
using EcoRoad.Alerting.Domain.Repositories;

namespace EcoRoad.Alerting.Application.Internal.CommandServices;

public class AlertingCommandService : IAlertingCommandService
{
    private readonly IAlertRepository _alerts;

    public AlertingCommandService(IAlertRepository alerts)
    {
        _alerts = alerts;
    }

    public async Task<ThresholdEvaluationResult> Handle(
        EvaluateThresholdDeviationCommand command,
        CancellationToken cancellationToken = default)
    {
        var assessment = DeviationAssessment.EvaluateThresholdDeviation(
            command.MeasurementId,
            command.MeasuredValue,
            command.ThresholdValue);

        if (assessment.MeasuredValue <= assessment.ThresholdValue)
        {
            return new ThresholdEvaluationResult(false, false, null);
        }

        var severity = assessment.IsCritical ? AlertSeverity.Critical : AlertSeverity.Warning;
        var alert = Alert.RaiseEnvironmentalAlert(
            assessment.Id,
            command.MonitoringPointId,
            severity,
            command.Message);

        await _alerts.AddAsync(alert, cancellationToken);
        return new ThresholdEvaluationResult(true, assessment.IsCritical, alert);
    }

    public async Task<Alert> Handle(
        AcknowledgeAlertCommand command,
        CancellationToken cancellationToken = default)
    {
        var alert = await _alerts.GetByIdAsync(command.AlertId, cancellationToken);
        if (alert == null)
        {
            throw new AlertingRuleException("Alert was not found.", notFound: true);
        }

        alert.AcknowledgeAlert(command.AcknowledgedBy);
        await _alerts.UpdateAsync(alert, cancellationToken);
        return alert;
    }
}
