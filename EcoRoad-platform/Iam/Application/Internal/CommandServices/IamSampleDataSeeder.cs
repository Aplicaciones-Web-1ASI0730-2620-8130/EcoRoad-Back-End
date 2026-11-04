using EcoRoad.Iam.Application.Internal.OutboundServices;
using EcoRoad.Iam.Domain.Model.Aggregates;
using EcoRoad.Iam.Domain.Model.ValueObjects;
using EcoRoad.Iam.Domain.Repositories;
using EcoRoad.Shared.Domain.Repositories;

namespace EcoRoad.Iam.Application.Internal.CommandServices;

/// <summary>
/// Seeds the demo company used by the frontend during local development.
/// </summary>
/// <remarks>
/// Collaborators are created through the same domain flow used in production:
/// the administrator signs up, invites, the invitation is accepted and access is granted.
/// </remarks>
public class IamSampleDataSeeder
{
    /// <summary>Identifier of the demo company shared with the frontend fixtures.</summary>
    public const string DemoCompanyId = "demo-company";

    /// <summary>Password of every demo user. Only for local development.</summary>
    public const string DemoPassword = "EcoRoadDemo123!";

    private readonly IUserRepository _users;
    private readonly IUserInvitationRepository _invitations;
    private readonly IHashingService _hashing;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<IamSampleDataSeeder> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="IamSampleDataSeeder" /> class.
    /// </summary>
    /// <param name="users">Repository of users.</param>
    /// <param name="invitations">Repository of invitations.</param>
    /// <param name="hashing">Service used to hash the demo password.</param>
    /// <param name="unitOfWork">Unit of work that persists the changes.</param>
    /// <param name="logger">Logger used to trace the seeding.</param>
    public IamSampleDataSeeder(
        IUserRepository users,
        IUserInvitationRepository invitations,
        IHashingService hashing,
        IUnitOfWork unitOfWork,
        ILogger<IamSampleDataSeeder> logger)
    {
        _users = users;
        _invitations = invitations;
        _hashing = hashing;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Creates the demo company users when the company has none.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    public async Task SeedIfEmptyAsync(CancellationToken cancellationToken = default)
    {
        if (await _users.ExistsByCompanyIdAsync(DemoCompanyId, cancellationToken))
        {
            return;
        }

        var passwordHash = _hashing.HashPassword(DemoPassword);
        var now = DateTime.UtcNow;

        var administrator = User.RegisterCompanyAdministrator(
            DemoCompanyId,
            "Carlos Mendoza",
            "c.mendoza@empresa.pe",
            passwordHash);
        await _users.AddAsync(administrator, cancellationToken);

        var collaborators = new[]
        {
            ("Ana Torres", "ana.torres@empresa.pe", Role.EnvironmentalEngineer),
            ("Luis Garcia", "luis.garcia@empresa.pe", Role.Inspector),
            ("Maria Lopez", "maria.lopez@empresa.pe", Role.FieldCollaborator)
        };

        foreach (var (name, email, role) in collaborators)
        {
            var invitation = new UserInvitation(DemoCompanyId, name, email, role, administrator.Id, now);
            invitation.Accept(now);
            await _invitations.AddAsync(invitation, cancellationToken);

            var collaborator = User.FromAcceptedInvitation(invitation, passwordHash);
            collaborator.GrantAccess(now);
            await _users.AddAsync(collaborator, cancellationToken);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);
        _logger.LogInformation("IAM demo data seeded for company {CompanyId}.", DemoCompanyId);
    }
}
