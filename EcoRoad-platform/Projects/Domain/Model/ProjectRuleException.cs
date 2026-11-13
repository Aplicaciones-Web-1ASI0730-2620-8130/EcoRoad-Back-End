namespace EcoRoad.Projects.Domain.Model;

public class ProjectRuleException : Exception
{
    public ProjectRuleException(string message, bool notFound = false)
        : base(message)
    {
        NotFound = notFound;
    }

    public bool NotFound { get; }
}
