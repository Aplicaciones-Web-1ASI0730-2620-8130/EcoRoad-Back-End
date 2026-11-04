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
/// Handles the commands that authenticate users and manage their access.
/// </summary>
public class UserCommandService : IUserCommandService
{
    private readonly IUserRepository _users;
    private readonly IHashingService _hashing;
    private readonly ITokenService _tokens;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserCommandService" /> class.
    /// </summary>
    /// <param name="users">Repository of users.</param>
    /// <param name="hashing">Service used to hash and verify passwords.</param>
    /// <param name="tokens">Service used to issue session tokens.</param>
    /// <param name="unitOfWork">Unit of work that persists the changes.</param>
    /// <param name="logger">Logger used to trace the handled commands.</param>
    public UserCommandService(
        IUserRepository users,
        IHashingService hashing,
        ITokenService tokens,
        IUnitOfWork unitOfWork,
        ILogger<UserCommandService> logger)
    {
        _users = users;
        _hashing = hashing;
        _tokens = tokens;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<User> Handle(
        SignUpCompanyAdministratorCommand command,
        CancellationToken cancellationToken = default)
    {
        var password = IamRules.RequireStrongPassword(command.Password);
        var companyId = IamRules.Require(command.CompanyId, "Company id", 36);

        if (await _users.ExistsByCompanyIdAsync(companyId, cancellationToken))
        {
            throw new IamRuleException(
                "COMPANY_ALREADY_REGISTERED",
                "The company already has an administrator. Ask for an invitation instead.",
                DomainErrorKind.Conflict);
        }

        var administrator = User.RegisterCompanyAdministrator(
            companyId,
            command.Name,
            command.Email,
            _hashing.HashPassword(password));

        await _users.AddAsync(administrator, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation(
            "Administrator {UserId} registered for company {CompanyId}.",
            administrator.Id,
            administrator.CompanyId);
        return administrator;
    }

    /// <inheritdoc />
    public async Task<AuthenticatedSession> Handle(SignInCommand command, CancellationToken cancellationToken = default)
    {
        var email = IamRules.RequireEmail(command.Email);
        var user = await FindSignInCandidateAsync(command.CompanyId, email, cancellationToken);

        if (user == null || !_hashing.VerifyPassword(command.Password ?? string.Empty, user.PasswordHash))
        {
            throw new IamRuleException(
                "INVALID_CREDENTIALS",
                "The email or the password is not correct.",
                DomainErrorKind.Unauthorized);
        }

        if (!user.CanSignIn)
        {
            throw new IamRuleException(
                "ACCESS_NOT_ACTIVE",
                "Your access has not been granted or was revoked. Contact your administrator.",
                DomainErrorKind.Forbidden);
        }

        _logger.LogInformation("User {UserId} signed in.", user.Id);
        return new AuthenticatedSession(user, _tokens.GenerateToken(user));
    }

    /// <inheritdoc />
    public async Task<User> Handle(GrantUserAccessCommand command, CancellationToken cancellationToken = default)
    {
        var (actor, target) = await LoadActorAndTargetAsync(command.ActorUserId, command.UserId, cancellationToken);
        AccessManagementPolicy.EnsureCanManage(actor, target);

        target.GrantAccess(DateTime.UtcNow);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Access granted to user {UserId} by {ActorId}.", target.Id, actor.Id);
        return target;
    }

    /// <inheritdoc />
    public async Task<User> Handle(RevokeUserAccessCommand command, CancellationToken cancellationToken = default)
    {
        var (actor, target) = await LoadActorAndTargetAsync(command.ActorUserId, command.UserId, cancellationToken);
        AccessManagementPolicy.EnsureCanManage(actor, target);
        AccessManagementPolicy.EnsureIsNotSelfRevocation(actor, target);

        target.RevokeAccess(DateTime.UtcNow);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Access revoked for user {UserId} by {ActorId}.", target.Id, actor.Id);
        return target;
    }

    /// <inheritdoc />
    public async Task<User> Handle(AssignUserRoleCommand command, CancellationToken cancellationToken = default)
    {
        var (actor, target) = await LoadActorAndTargetAsync(command.ActorUserId, command.UserId, cancellationToken);
        AccessManagementPolicy.EnsureCanManage(actor, target);
        AccessManagementPolicy.EnsureKeepsOwnAdministratorRole(actor, target, command.Role);

        target.AssignRole(command.Role);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Role {Role} assigned to user {UserId}.", target.Role, target.Id);
        return target;
    }

    /// <inheritdoc />
    public async Task<User> Handle(UpdateUserPermissionsCommand command, CancellationToken cancellationToken = default)
    {
        var permissions = command.Permissions ?? Array.Empty<Domain.Model.ValueObjects.Permission>();
        var (actor, target) = await LoadActorAndTargetAsync(command.ActorUserId, command.UserId, cancellationToken);
        AccessManagementPolicy.EnsureCanManage(actor, target);
        AccessManagementPolicy.EnsureKeepsOwnUserManagement(actor, target, permissions);

        target.UpdatePermissions(permissions);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Permissions of user {UserId} updated.", target.Id);
        return target;
    }

    /// <summary>
    /// Resolves the user that is trying to sign in.
    /// </summary>
    /// <param name="companyId">Optional company identifier.</param>
    /// <param name="email">Normalized email address.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The matching user, or <c>null</c> when none matches.</returns>
    private async Task<User?> FindSignInCandidateAsync(
        string? companyId,
        string email,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(companyId))
        {
            return await _users.FindByCompanyIdAndEmailAsync(companyId.Trim(), email, cancellationToken);
        }

        var matches = await _users.ListByEmailAsync(email, cancellationToken);
        if (matches.Count > 1)
        {
            throw new IamRuleException(
                "COMPANY_REQUIRED",
                "The email belongs to several companies. Select the company to sign in.");
        }

        return matches.FirstOrDefault();
    }

    /// <summary>
    /// Loads the acting administrator and the collaborator being managed.
    /// </summary>
    /// <param name="actorUserId">Identifier of the acting administrator.</param>
    /// <param name="userId">Identifier of the collaborator.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The actor and the target users.</returns>
    private async Task<(User Actor, User Target)> LoadActorAndTargetAsync(
        string actorUserId,
        string userId,
        CancellationToken cancellationToken)
    {
        var actor = await _users.FindByIdAsync(actorUserId, cancellationToken)
            ?? throw new IamRuleException(
                "UNAUTHENTICATED",
                "The session user no longer exists.",
                DomainErrorKind.Unauthorized);

        var target = await _users.FindByIdAsync(userId, cancellationToken)
            ?? throw new IamRuleException("USER_NOT_FOUND", "The collaborator was not found.", DomainErrorKind.NotFound);

        return (actor, target);
    }
}
