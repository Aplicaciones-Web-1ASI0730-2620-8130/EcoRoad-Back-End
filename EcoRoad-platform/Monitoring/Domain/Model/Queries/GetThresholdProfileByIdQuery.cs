namespace EcoRoad.Monitoring.Domain.Model.Queries;

/// <summary>
/// Retrieves a threshold profile by its identifier.
/// </summary>
/// <param name="ThresholdProfileId">Identifier of the profile.</param>
public record GetThresholdProfileByIdQuery(string ThresholdProfileId);
