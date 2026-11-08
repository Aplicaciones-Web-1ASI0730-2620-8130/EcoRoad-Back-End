using EcoRoad.Alerting.Domain.Model.Aggregates;
using EcoRoad.Alerting.Domain.Model.ValueObjects;
using EcoRoad.Alerting.Domain.Repositories;

namespace EcoRoad.Alerting.Application.Internal.CommandServices;

/// <summary>
///     Seeds one warning path and one critical alert when the store has no assessments.
/// </summary>
public class AlertingSampleDataSeeder
{
    private readonly IDeviationAssessmentRepository _assessments;
    private readonly IAlertRepository _alerts;

    public AlertingSampleDataSeeder(
        IDeviationAssessmentRepository assessments,
        IAlertRepository alerts)
    {
        _assessments = assessments;
        _alerts = alerts;
    }

    public async Task SeedIfEmptyAsync(CancellationToken cancellationToken = default)
    {
        var existing = await _assessments.ListAsync(null, cancellationToken);
        if (existing.Count > 0)
        {
            return;
        }

        var monitoringPointId = "11111111-1111-1111-1111-111111111111";
        var withinLimit = DeviationAssessment.EvaluateThresholdDeviation(
            "22222222-2222-2222-2222-222222222222",
            40,
            80);
        var critical = DeviationAssessment.EvaluateThresholdDeviation(
            "33333333-3333-3333-3333-333333333333",
            120,
            80);

        await _assessments.AddAsync(withinLimit, cancellationToken);
        await _assessments.AddAsync(critical, cancellationToken);

        var alert = Alert.RaiseEnvironmentalAlert(
            critical.Id,
            monitoringPointId,
            AlertSeverity.Critical,
            "Particulate matter exceeded the threshold.");
        await _alerts.AddAsync(alert, cancellationToken);
    }
}
