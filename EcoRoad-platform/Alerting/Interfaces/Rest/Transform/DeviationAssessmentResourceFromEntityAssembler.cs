using EcoRoad.Alerting.Domain.Model.Aggregates;
using EcoRoad.Alerting.Domain.Model.Queries;
using EcoRoad.Alerting.Interfaces.Rest.Resources;

namespace EcoRoad.Alerting.Interfaces.Rest.Transform;

public static class DeviationAssessmentResourceFromEntityAssembler
{
    public static DeviationAssessmentResource ToResourceFromEntity(DeviationAssessment assessment)
    {
        return new DeviationAssessmentResource(
            assessment.Id,
            assessment.MeasurementId,
            assessment.MeasuredValue,
            assessment.ThresholdValue,
            assessment.IsCritical,
            assessment.AssessedAt);
    }

    public static DeviationAssessmentResource ToResourceFromView(DeviationAssessmentView view)
    {
        return new DeviationAssessmentResource(
            view.Id,
            view.MeasurementId,
            view.MeasuredValue,
            view.ThresholdValue,
            view.IsCritical,
            view.AssessedAt);
    }

    public static IEnumerable<DeviationAssessmentResource> ToResourcesFromViews(
        IEnumerable<DeviationAssessmentView> views)
    {
        return views.Select(ToResourceFromView);
    }
}
