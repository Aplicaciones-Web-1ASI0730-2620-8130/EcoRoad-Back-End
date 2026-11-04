using EcoRoad.Monitoring.Domain.Model.ValueObjects;

namespace EcoRoad.Monitoring.Domain.Model.Queries;

/// <summary>
/// Dashboard indicator of a road section: its current readings and its worst status.
/// </summary>
/// <param name="SectionId">Identifier of the road section.</param>
/// <param name="Status">Worst status of the current readings.</param>
/// <param name="Readings">Latest reading of every parameter measured in the section.</param>
public record SectionIndicator(string SectionId, MonitoringStatus Status, IReadOnlyList<ReadingSnapshot> Readings);
