using EcoRoad.Shared.Domain.Model.Entities;
using EcoRoad.Shared.Domain.Model.Exceptions;

namespace EcoRoad.Assets.Domain.Model.Aggregates;

/// <summary>
/// Historical record of a sensor installed at a monitoring point.
/// </summary>
/// <remarks>
/// Keeping the history lets auditors know which device produced every measurement of a point.
/// </remarks>
public class SensorAssignment : IAuditableEntity
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    private SensorAssignment()
    {
    }

    /// <summary>
    /// Opens the assignment of a sensor to a monitoring point.
    /// </summary>
    /// <param name="sensorId">Identifier of the sensor.</param>
    /// <param name="monitoringPointId">Identifier of the monitoring point.</param>
    /// <param name="assignedAt">UTC date of the installation.</param>
    public SensorAssignment(string sensorId, string monitoringPointId, DateTime assignedAt)
    {
        Id = Guid.NewGuid().ToString();
        SensorId = AssetsRules.Require(sensorId, "Sensor id", 36, "INVALID_ASSIGNMENT");
        MonitoringPointId = AssetsRules.Require(monitoringPointId, "Monitoring point id", 36, "INVALID_ASSIGNMENT");
        AssignedAt = assignedAt;
    }

    /// <summary>Gets the unique identifier of the assignment.</summary>
    public string Id { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the sensor.</summary>
    public string SensorId { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the monitoring point.</summary>
    public string MonitoringPointId { get; private set; } = string.Empty;

    /// <summary>Gets the UTC date of the installation.</summary>
    public DateTime AssignedAt { get; private set; }

    /// <summary>Gets the UTC date when the sensor left the point, if it did.</summary>
    public DateTime? ReleasedAt { get; private set; }

    /// <inheritdoc />
    public DateTime? CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets a value indicating whether the sensor is still installed at the point.
    /// </summary>
    public bool IsActive => ReleasedAt == null;

    /// <summary>
    /// Closes the assignment.
    /// </summary>
    /// <param name="releasedAt">UTC date when the sensor left the point.</param>
    public void Close(DateTime releasedAt)
    {
        if (!IsActive)
        {
            throw new AssetsRuleException("ASSIGNMENT_CLOSED", "The assignment is already closed.", DomainErrorKind.Conflict);
        }

        ReleasedAt = releasedAt;
    }
}
