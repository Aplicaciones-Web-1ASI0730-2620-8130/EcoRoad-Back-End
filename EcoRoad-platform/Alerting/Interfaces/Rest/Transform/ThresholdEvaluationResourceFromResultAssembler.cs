using EcoRoad.Alerting.Domain.Model.Commands;
using EcoRoad.Alerting.Interfaces.Rest.Resources;

namespace EcoRoad.Alerting.Interfaces.Rest.Transform;

public static class ThresholdEvaluationResourceFromResultAssembler
{
    public static ThresholdEvaluationResource ToResourceFromResult(ThresholdEvaluationResult result)
    {
        return new ThresholdEvaluationResource(
            result.DeviationDetected,
            result.IsCritical,
            DeviationAssessmentResourceFromEntityAssembler.ToResourceFromEntity(result.Assessment),
            result.Alert == null ? null : AlertResourceFromEntityAssembler.ToResourceFromEntity(result.Alert));
    }
}
