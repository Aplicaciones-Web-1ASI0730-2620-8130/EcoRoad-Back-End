namespace EcoRoad.Iam.Domain.Model.ValueObjects;

/// <summary>
/// Roles a collaborator can hold inside a company. Each role grants a default set of permissions.
/// </summary>
public enum Role
{
    /// <summary>Administrator of the company account; manages collaborators and their access.</summary>
    CompanyAdmin = 1,

    /// <summary>Site director responsible for the road works.</summary>
    SiteDirector = 2,

    /// <summary>Environmental engineer that supervises the environmental indicators.</summary>
    EnvironmentalEngineer = 3,

    /// <summary>Inspector that audits the work fronts on site.</summary>
    Inspector = 4,

    /// <summary>Field collaborator that records evidence on site.</summary>
    FieldCollaborator = 5
}
