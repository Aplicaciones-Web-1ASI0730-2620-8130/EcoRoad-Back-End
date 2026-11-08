using EcoRoad.Alerting.Domain.Model.Commands;
using EcoRoad.Alerting.Interfaces.Rest.Resources;

namespace EcoRoad.Alerting.Interfaces.Rest.Transform;

public static class EvaluateThresholdDeviationCommandFromResourceAssembler
{
    public static EvaluateThresholdDeviationCommand ToCommandFromResource(EvaluateThresholdDeviationResource resource)
    {
        return new EvaluateThresholdDeviationCommand(
            resource.MeasurementId,
            resource.MonitoringPointId,
            resource.MeasuredValue,
            resource.ThresholdValue,
            resource.Message);
    }
}
