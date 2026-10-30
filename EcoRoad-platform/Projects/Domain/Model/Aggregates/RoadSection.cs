using EcoRoad.Projects.Domain.Model;

namespace EcoRoad.Projects.Domain.Model.Aggregates;

public class RoadSection
{
    public RoadSection()
    {
        WorkFronts = new List<WorkFront>();
    }

    public RoadSection(string projectId, string name, double startKilometer, double endKilometer)
    {
        Id = Guid.NewGuid().ToString();
        ProjectId = ProjectRules.Require(projectId, "Project id", 36);
        Name = ProjectRules.Require(name, "Name", 100);
        ProjectRules.RequireKilometerRange(startKilometer, endKilometer);
        StartKilometer = startKilometer;
        EndKilometer = endKilometer;
        WorkFronts = new List<WorkFront>();
    }

    public void Update(string name, double startKilometer, double endKilometer)
    {
        Name = ProjectRules.Require(name, "Name", 100);
        ProjectRules.RequireKilometerRange(startKilometer, endKilometer);
        StartKilometer = startKilometer;
        EndKilometer = endKilometer;
    }

    public string Id { get; set; } = string.Empty;

    public string ProjectId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public double StartKilometer { get; set; }

    public double EndKilometer { get; set; }

    public RoadProject? Project { get; set; }

    public ICollection<WorkFront> WorkFronts { get; set; } = new List<WorkFront>();
}
