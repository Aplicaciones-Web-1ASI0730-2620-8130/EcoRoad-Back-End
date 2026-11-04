namespace EcoRoad.Monitoring.Domain.Model.Queries;

/// <summary>
/// Retrieves a measurement by its identifier.
/// </summary>
/// <param name="MeasurementId">Identifier of the measurement.</param>
public record GetMeasurementByIdQuery(string MeasurementId);
