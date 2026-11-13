using EcoRoad.Alerting.Application.CommandServices;
using EcoRoad.Alerting.Domain.Model;
using EcoRoad.Alerting.Domain.Model.Aggregates;
using EcoRoad.Alerting.Domain.Model.Commands;
using EcoRoad.Alerting.Domain.Model.ValueObjects;
using EcoRoad.Alerting.Domain.Repositories;
using EcoRoad.Projects.Domain.Repositories;

namespace EcoRoad.Alerting.Application.Internal.CommandServices;

public class AlertingCommandService : IAlertingCommandService
{
    private readonly IAlertRuleRepository _rules;
    private readonly IAlertRepository _alerts;
    private readonly IWorkFrontRepository _workFronts;

    public AlertingCommandService(
        IAlertRuleRepository rules,
        IAlertRepository alerts,
        IWorkFrontRepository workFronts)
    {
        _rules = rules;
        _alerts = alerts;
        _workFronts = workFronts;
    }

    public async Task<AlertRule> Handle(
        RegisterAlertRuleCommand command,
        CancellationToken cancellationToken = default)
    {
        var rule = new AlertRule(
            command.Parameter,
            command.Unit,
            command.TimeBand,
            command.Limit,
            command.CriticalExcess);

        var existing = await _rules.GetByParameterAndTimeBandAsync(rule.Parameter, rule.TimeBand, cancellationToken);
        if (existing != null)
        {
            throw new AlertingRuleException("An alert rule for this parameter and time band is already registered.");
        }

        await _rules.AddAsync(rule, cancellationToken);
        return rule;
    }

    public async Task<ThresholdEvaluationResult> Handle(
        EvaluateThresholdDeviationCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Parameter))
        {
            throw new AlertingRuleException("Parameter is required.");
        }

        if (await _workFronts.GetByIdAsync(command.WorkFrontId, cancellationToken) == null)
        {
            throw new AlertingRuleException("Work front was not found.", notFound: true);
        }

        var rule = await _rules.GetByParameterAndTimeBandAsync(
            command.Parameter.Trim(),
            command.TimeBand,
            cancellationToken);
        if (rule == null)
        {
            throw new AlertingRuleException("Alert rule was not found.", notFound: true);
        }

        var assessment = rule.Evaluate(command.MeasuredValue, command.TimeBand);
        if (!assessment.IsDeviation)
        {
            return new ThresholdEvaluationResult(false, false, null);
        }

        var severity = assessment.IsCritical ? AlertSeverity.Critical : AlertSeverity.Warning;
        var alert = new Alert(
            command.WorkFrontId,
            rule.Id,
            rule.Parameter,
            rule.Unit,
            rule.TimeBand,
            assessment.MeasuredValue,
            assessment.LimitValue,
            severity);

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

        alert.Acknowledge(command.AcknowledgedBy);
        await _alerts.UpdateAsync(alert, cancellationToken);
        return alert;
    }
}
