namespace EcoRoad.Projects.Domain.Entities;

public class RoadProject
{
    public string Id { get; set; } = string.Empty;

    public string CompanyId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public ICollection<RoadSection> Sections { get; set; } = new List<RoadSection>();
}
