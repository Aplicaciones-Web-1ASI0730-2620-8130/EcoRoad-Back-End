namespace EcoRoad.Assets.Domain.Model.Queries;

/// <summary>
/// Computes the deployment indicators of the sensor fleet.
/// </summary>
/// <param name="ProjectId">Optional road project filter.</param>
public record GetAssetSummaryQuery(string? ProjectId);
