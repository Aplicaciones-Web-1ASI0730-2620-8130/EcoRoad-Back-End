namespace EcoRoad.Projects.Domain.Model.Aggregates;

public class RoadSection
{
    public string Id { get; set; } = string.Empty;

    public string ProjectId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public double StartKilometer { get; set; }

    public double EndKilometer { get; set; }

    public RoadProject? Project { get; set; }

    public ICollection<WorkFront> WorkFronts { get; set; } = new List<WorkFront>();
}
