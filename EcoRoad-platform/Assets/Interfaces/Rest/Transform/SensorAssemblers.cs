using EcoRoad.Assets.Domain.Model.Aggregates;
using EcoRoad.Assets.Domain.Model.Commands;
using EcoRoad.Assets.Domain.Model.Queries;
using EcoRoad.Assets.Interfaces.Rest.Resources;

namespace EcoRoad.Assets.Interfaces.Rest.Transform;

/// <summary>
/// Converts <see cref="Sensor" /> aggregates and their read models into resources.
/// </summary>
public static class SensorResourceFromEntityAssembler
{
    /// <summary>
    /// Converts a sensor into its resource, evaluating the reporting policy at the current time.
    /// </summary>
    /// <param name="sensor">The sensor to convert.</param>
    /// <returns>The sensor resource.</returns>
    public static SensorResource ToResourceFromEntity(Sensor sensor)
    {
        return new SensorResource(
            sensor.Id,
            sensor.Code,
            sensor.Name,
            sensor.SerialNumber,
            sensor.Type,
            sensor.Model,
            sensor.Status,
            sensor.MonitoringPointId,
            sensor.ProjectId,
            sensor.SectionId,
            sensor.CalibratedAt,
            sensor.CalibrationValidUntil,
            sensor.ActivatedAt,
            sensor.CanReport(DateTime.UtcNow));
    }

    /// <summary>
    /// Converts a list of sensors into resources.
    /// </summary>
    /// <param name="sensors">The sensors to convert.</param>
    /// <returns>The sensor resources.</returns>
    public static IReadOnlyList<SensorResource> ToResourcesFromEntities(IEnumerable<Sensor> sensors)
    {
        return sensors.Select(ToResourceFromEntity).ToList();
    }

    /// <summary>
    /// Converts an assignment into its resource.
    /// </summary>
    /// <param name="assignment">The assignment to convert.</param>
    /// <returns>The assignment resource.</returns>
    public static SensorAssignmentResource ToAssignmentResource(SensorAssignment assignment)
    {
        return new SensorAssignmentResource(
            assignment.Id,
            assignment.SensorId,
            assignment.MonitoringPointId,
            assignment.AssignedAt,
            assignment.ReleasedAt,
            assignment.IsActive);
    }

    /// <summary>
    /// Converts the fleet summary into its resource.
    /// </summary>
    /// <param name="summary">The fleet summary.</param>
    /// <returns>The summary resource.</returns>
    public static AssetSummaryResource ToSummaryResource(AssetSummary summary)
    {
        return new AssetSummaryResource(
            summary.Total,
            summary.ReadyToReport,
            summary.Assigned,
            summary.Available,
            summary.CalibrationDue);
    }
}

/// <summary>
/// Converts sensor resources into commands.
/// </summary>
public static class SensorCommandFromResourceAssembler
{
    /// <summary>
    /// Builds the registration command.
    /// </summary>
    /// <param name="resource">The request body.</param>
    /// <returns>The command.</returns>
    public static RegisterSensorCommand ToRegisterCommand(RegisterSensorResource resource)
    {
        return new RegisterSensorCommand(resource.Name, resource.SerialNumber, resource.Type, resource.Model);
    }

    /// <summary>
    /// Builds the assignment command.
    /// </summary>
    /// <param name="sensorId">Identifier of the sensor.</param>
    /// <param name="resource">The request body.</param>
    /// <returns>The command.</returns>
    public static AssignSensorCommand ToAssignCommand(string sensorId, AssignSensorResource resource)
    {
        return new AssignSensorCommand(sensorId, resource.MonitoringPointId);
    }

    /// <summary>
    /// Builds the calibration command.
    /// </summary>
    /// <param name="sensorId">Identifier of the sensor.</param>
    /// <param name="resource">The request body.</param>
    /// <returns>The command.</returns>
    public static CalibrateSensorCommand ToCalibrateCommand(string sensorId, CalibrateSensorResource resource)
    {
        return new CalibrateSensorCommand(sensorId, resource.ValidUntil);
    }
}
