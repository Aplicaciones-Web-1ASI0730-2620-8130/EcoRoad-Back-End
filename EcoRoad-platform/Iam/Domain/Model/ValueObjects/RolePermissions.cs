namespace EcoRoad.Iam.Domain.Model.ValueObjects;

/// <summary>
/// Catalog of the default permissions granted by each <see cref="Role" />.
/// </summary>
public static class RolePermissions
{
    private static readonly IReadOnlyDictionary<Role, IReadOnlyList<Permission>> Defaults =
        new Dictionary<Role, IReadOnlyList<Permission>>
        {
            [Role.CompanyAdmin] = Enum.GetValues<Permission>(),
            [Role.SiteDirector] = new[]
            {
                Permission.ViewProjects,
                Permission.ConsultSensors,
                Permission.ConsultIndicators,
                Permission.ConsultAlerts,
                Permission.ManageIncidents,
                Permission.RegisterCorrectiveActions,
                Permission.GenerateReports
            },
            [Role.EnvironmentalEngineer] = new[]
            {
                Permission.ViewProjects,
                Permission.ConsultSensors,
                Permission.ConsultIndicators,
                Permission.ConsultAlerts,
                Permission.ManageIncidents,
                Permission.GenerateReports
            },
            [Role.Inspector] = new[]
            {
                Permission.ViewProjects,
                Permission.ConsultSensors,
                Permission.ConsultIndicators,
                Permission.ConsultAlerts,
                Permission.ManageIncidents,
                Permission.RegisterFieldEvidence
            },
            [Role.FieldCollaborator] = new[]
            {
                Permission.ViewProjects,
                Permission.ConsultAlerts,
                Permission.RegisterFieldEvidence
            }
        };

    /// <summary>
    /// Gets the default permissions of a role.
    /// </summary>
    /// <param name="role">The role to look up.</param>
    /// <returns>A new list with the default permissions of the role.</returns>
    public static List<Permission> DefaultsFor(Role role)
    {
        return Defaults[role].ToList();
    }
}
