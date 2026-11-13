using EcoRoad.Projects.Domain.Model;

namespace EcoRoad.Projects.Domain.Model.Aggregates;

public class WorkFront
{
    public WorkFront()
    {
    }

    public WorkFront(string sectionId, string name, string? status)
    {
        Id = Guid.NewGuid().ToString();
        SectionId = ProjectRules.Require(sectionId, "Section id", 36);
        Name = ProjectRules.Require(name, "Name", 100);
        Status = ProjectRules.RegisteredStatus(status);
    }

    public string Id { get; set; } = string.Empty;

    public string SectionId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public RoadSection? Section { get; set; }
}
