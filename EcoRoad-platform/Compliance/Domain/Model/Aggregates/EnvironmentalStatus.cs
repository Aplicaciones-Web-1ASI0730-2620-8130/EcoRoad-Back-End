using EcoRoad.Compliance.Domain.Model;
using EcoRoad.Compliance.Domain.Model.ValueObjects;

namespace EcoRoad.Compliance.Domain.Model.Aggregates;

public class EnvironmentalStatus
{
    public EnvironmentalStatus()
    {
    }

    public EnvironmentalStatus(ComplianceScope scope, string subjectId, EnvironmentalStanding standing)
    {
        Id = Guid.NewGuid().ToString();
        Scope = scope;
        SubjectId = ComplianceRules.RequireId(subjectId, "Subject id");
        Standing = standing;
        UpdatedAt = DateTime.UtcNow;
    }

    public string Id { get; set; } = string.Empty;

    public ComplianceScope Scope { get; set; }

    public string SubjectId { get; set; } = string.Empty;

    public EnvironmentalStanding Standing { get; set; }

    public DateTime UpdatedAt { get; set; }

    public void Update(EnvironmentalStanding standing)
    {
        Standing = standing;
        UpdatedAt = DateTime.UtcNow;
    }
}
