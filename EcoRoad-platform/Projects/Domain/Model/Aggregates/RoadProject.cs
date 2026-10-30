using EcoRoad.Projects.Domain.Model;

namespace EcoRoad.Projects.Domain.Model.Aggregates;

public class RoadProject
{
    public RoadProject()
    {
        Sections = new List<RoadSection>();
    }

    public RoadProject(string companyId, string name, string code, string? status)
    {
        Id = Guid.NewGuid().ToString();
        CompanyId = ProjectRules.Require(companyId, "Company id", 3);
        Name = ProjectRules.Require(name, "Name", 100);
        Code = ProjectRules.Require(code, "Code", 50);
        Status = ProjectRules.RegisteredStatus(status);
        CreatedAt = DateTime.UtcNow;
        Sections = new List<RoadSection>();
    }

    public string Id { get; set; } = string.Empty;

    public string CompanyId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public ICollection<RoadSection> Sections { get; set; } = new List<RoadSection>();
}
