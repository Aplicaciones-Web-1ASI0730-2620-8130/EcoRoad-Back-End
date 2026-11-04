namespace EcoRoad.Iam.Domain.Model.ValueObjects;

/// <summary>
/// Fine grained permissions that authorize the operations of the EcoRoad platform.
/// </summary>
public enum Permission
{
    /// <summary>Consult road projects and their sections.</summary>
    ViewProjects = 1,

    /// <summary>Consult sensors and monitoring points.</summary>
    ConsultSensors = 2,

    /// <summary>Consult environmental measurements.</summary>
    ConsultIndicators = 3,

    /// <summary>Consult environmental alerts.</summary>
    ConsultAlerts = 4,

    /// <summary>Open, assign, resolve and close environmental incidents.</summary>
    ManageIncidents = 5,

    /// <summary>Register corrective actions for an incident.</summary>
    RegisterCorrectiveActions = 6,

    /// <summary>Register field evidence for an incident.</summary>
    RegisterFieldEvidence = 7,

    /// <summary>Generate compliance reports.</summary>
    GenerateReports = 8,

    /// <summary>Invite collaborators and manage their roles, permissions and access.</summary>
    ManageUsers = 9
}
