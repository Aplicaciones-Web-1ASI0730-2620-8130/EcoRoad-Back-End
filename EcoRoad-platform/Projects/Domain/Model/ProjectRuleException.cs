namespace EcoRoad.Projects.Domain.Model;

public class ProjectRuleException : Exception
{
    public ProjectRuleException(string message)
        : base(message)
    {
    }
}
