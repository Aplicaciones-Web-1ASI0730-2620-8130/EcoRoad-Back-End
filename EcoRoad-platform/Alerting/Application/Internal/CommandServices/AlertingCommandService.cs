using EcoRoad.Alerting.Application.CommandServices;
using EcoRoad.Alerting.Domain.Model;
using EcoRoad.Alerting.Domain.Model.Aggregates;
using EcoRoad.Alerting.Domain.Model.Commands;
using EcoRoad.Alerting.Domain.Model.ValueObjects;
using EcoRoad.Alerting.Domain.Repositories;

namespace EcoRoad.Alerting.Application.Internal.CommandServices;

/// <summary>
///     Handles threshold evaluation and the alert lifecycle.
/// </summary>
public class AlertingCommandService : IAlertingCommandService
{
    private readonly IDeviationAssessmentRepository _assessments;
    private readonly IAlertRepository _alerts;

    public AlertingCommandService(
        IDeviationAssessmentRepository assessments,
        IAlertRepository alerts)
    {
        _assessments = assessments;
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
        await _assessments.AddAsync(assessment, cancellationToken);

        if (assessment.MeasuredValue <= assessment.ThresholdValue)
        {
            return new ThresholdEvaluationResult(false, false, null);
        }

        var alert = await RaiseAsync(
            new RaiseEnvironmentalAlertCommand(
                assessment.Id,
                command.MonitoringPointId,
                assessment.IsCritical ? AlertSeverity.Critical : AlertSeverity.Warning,
                command.Message),
            cancellationToken);

        return new ThresholdEvaluationResult(true, assessment.IsCritical, alert);
    }

    public async Task<DeviationAssessment> Handle(
        ConfirmCriticalDeviationCommand command,
        CancellationToken cancellationToken = default)
    {
        var assessment = await _assessments.FindByIdAsync(command.AssessmentId, cancellationToken);
        if (assessment == null)
        {
            throw new AlertingRuleException("Deviation assessment was not found.", notFound: true);
        }

        assessment.ConfirmCriticalDeviation();
        await _assessments.UpdateAsync(assessment, cancellationToken);
        return assessment;
    }

    public Task<Alert> Handle(
        RaiseEnvironmentalAlertCommand command,
        CancellationToken cancellationToken = default)
    {
        return RaiseAsync(command, cancellationToken);
    }

    public async Task<Alert> Handle(
        AcknowledgeAlertCommand command,
        CancellationToken cancellationToken = default)
    {
        var alert = await _alerts.FindByIdAsync(command.AlertId, cancellationToken);
        if (alert == null)
        {
            throw new AlertingRuleException("Alert was not found.", notFound: true);
        }

        alert.AcknowledgeAlert(command.UserId);
        await _alerts.UpdateAsync(alert, cancellationToken);
        return alert;
    }

    private async Task<Alert> RaiseAsync(
        RaiseEnvironmentalAlertCommand command,
        CancellationToken cancellationToken)
    {
        var assessment = await _assessments.FindByIdAsync(command.AssessmentId, cancellationToken);
        if (assessment == null)
        {
            throw new AlertingRuleException("Deviation assessment was not found.", notFound: true);
        }

        if (assessment.MeasuredValue <= assessment.ThresholdValue)
        {
            throw new AlertingRuleException("An alert can only be raised for a threshold deviation.");
        }

        var alert = Alert.RaiseEnvironmentalAlert(
            assessment.Id,
            command.MonitoringPointId,
            command.Severity,
            command.Message);
        await _alerts.AddAsync(alert, cancellationToken);
        return alert;
    }
}
