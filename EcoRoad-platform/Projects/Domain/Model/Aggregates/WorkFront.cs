namespace EcoRoad.Projects.Domain.Model.Aggregates;

public class WorkFront
{
    public string Id { get; set; } = string.Empty;

    public string SectionId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public RoadSection? Section { get; set; }
}
