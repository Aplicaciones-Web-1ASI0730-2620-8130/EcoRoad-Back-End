using EcoRoad.Alerting.Domain.Model.Commands;
using EcoRoad.Alerting.Interfaces.Rest.Resources;

namespace EcoRoad.Alerting.Interfaces.Rest.Transform;

public static class RaiseEnvironmentalAlertCommandFromResourceAssembler
{
    public static RaiseEnvironmentalAlertCommand ToCommandFromResource(RaiseEnvironmentalAlertResource resource)
    {
        return new RaiseEnvironmentalAlertCommand(
            resource.AssessmentId,
            resource.MonitoringPointId,
            resource.Severity,
            resource.Message);
    }
}
