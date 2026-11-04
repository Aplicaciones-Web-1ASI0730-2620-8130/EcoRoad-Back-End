using EcoRoad.Iam.Application.CommandServices;
using EcoRoad.Iam.Application.Internal.OutboundServices;
using EcoRoad.Iam.Domain.Model;
using EcoRoad.Iam.Domain.Model.Aggregates;
using EcoRoad.Iam.Domain.Model.Commands;
using EcoRoad.Iam.Domain.Repositories;
using EcoRoad.Iam.Domain.Services;
using EcoRoad.Shared.Domain.Model.Exceptions;
using EcoRoad.Shared.Domain.Repositories;

namespace EcoRoad.Iam.Application.Internal.CommandServices;

/// <summary>
/// Handles the commands that invite collaborators and accept invitations.
/// </summary>
public class UserInvitationCommandService : IUserInvitationCommandService
{
    private readonly IUserInvitationRepository _invitations;
    private readonly IUserRepository _users;
    private readonly IHashingService _hashing;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserInvitationCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserInvitationCommandService" /> class.
    /// </summary>
    /// <param name="invitations">Repository of invitations.</param>
    /// <param name="users">Repository of users.</param>
    /// <param name="hashing">Service used to hash passwords.</param>
    /// <param name="unitOfWork">Unit of work that persists the changes.</param>
    /// <param name="logger">Logger used to trace the handled commands.</param>
    public UserInvitationCommandService(
        IUserInvitationRepository invitations,
        IUserRepository users,
        IHashingService hashing,
        IUnitOfWork unitOfWork,
        ILogger<UserInvitationCommandService> logger)
    {
        _invitations = invitations;
        _users = users;
        _hashing = hashing;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<UserInvitation> Handle(InviteUserCommand command, CancellationToken cancellationToken = default)
    {
        var actor = await _users.FindByIdAsync(command.ActorUserId, cancellationToken)
            ?? throw new IamRuleException(
                "UNAUTHENTICATED",
                "The session user no longer exists.",
                DomainErrorKind.Unauthorized);
        AccessManagementPolicy.EnsureIsAdministrator(actor);

        var now = DateTime.UtcNow;
        var invitation = new UserInvitation(actor.CompanyId, command.Name, command.Email, command.Role, actor.Id, now);

        if (await _users.FindByCompanyIdAndEmailAsync(actor.CompanyId, invitation.Email, cancellationToken) != null)
        {
            throw new IamRuleException(
                "DUPLICATE_EMAIL",
                "This email is already registered in the company.",
                DomainErrorKind.Conflict);
        }

        var pending = await _invitations.FindPendingByCompanyIdAndEmailAsync(
            actor.CompanyId,
            invitation.Email,
            cancellationToken);
        if (pending != null && !pending.ExpireIfOverdue(now))
        {
            throw new IamRuleException(
                "INVITATION_ALREADY_SENT",
                "There is already a pending invitation for this email.",
                DomainErrorKind.Conflict);
        }

        await _invitations.AddAsync(invitation, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation(
            "Invitation {InvitationId} sent by {ActorId} for company {CompanyId}.",
            invitation.Id,
            actor.Id,
            actor.CompanyId);
        return invitation;
    }

    /// <inheritdoc />
    public async Task<User> Handle(AcceptInvitationCommand command, CancellationToken cancellationToken = default)
    {
        var password = IamRules.RequireStrongPassword(command.Password);
        var invitation = await _invitations.FindByIdAsync(command.InvitationId, cancellationToken)
            ?? throw new IamRuleException(
                "INVITATION_NOT_FOUND",
                "The invitation was not found.",
                DomainErrorKind.NotFound);

        var now = DateTime.UtcNow;
        if (invitation.ExpireIfOverdue(now))
        {
            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        invitation.Accept(now);

        if (await _users.FindByCompanyIdAndEmailAsync(invitation.CompanyId, invitation.Email, cancellationToken) != null)
        {
            throw new IamRuleException(
                "DUPLICATE_EMAIL",
                "This email is already registered in the company.",
                DomainErrorKind.Conflict);
        }

        var user = User.FromAcceptedInvitation(invitation, _hashing.HashPassword(password));
        await _users.AddAsync(user, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation(
            "Invitation {InvitationId} accepted; user {UserId} awaits access.",
            invitation.Id,
            user.Id);
        return user;
    }
}
