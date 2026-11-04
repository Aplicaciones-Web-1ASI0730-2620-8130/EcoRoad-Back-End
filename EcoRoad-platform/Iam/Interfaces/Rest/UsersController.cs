using EcoRoad.Iam.Application.CommandServices;
using EcoRoad.Iam.Application.QueryServices;
using EcoRoad.Iam.Domain.Model;
using EcoRoad.Iam.Domain.Model.Commands;
using EcoRoad.Iam.Domain.Model.Queries;
using EcoRoad.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using EcoRoad.Iam.Infrastructure.Pipeline.Middleware.Extensions;
using EcoRoad.Iam.Interfaces.Rest.Resources;
using EcoRoad.Iam.Interfaces.Rest.Transform;
using EcoRoad.Shared.Domain.Model.Exceptions;
using EcoRoad.Shared.Interfaces.Rest.Resources;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoad.Iam.Interfaces.Rest;

/// <summary>
/// Endpoints to consult the collaborators of a company and manage their access.
/// Every operation is limited to the company of the authenticated user.
/// </summary>
[ApiController]
[Route("api/iam/users")]
[Produces("application/json")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserCommandService _commands;
    private readonly IUserQueryService _queries;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController" /> class.
    /// </summary>
    /// <param name="commands">Command service of users.</param>
    /// <param name="queries">Query service of users.</param>
    public UsersController(IUserCommandService commands, IUserQueryService queries)
    {
        _commands = commands;
        _queries = queries;
    }

    /// <summary>
    /// Lists the collaborators of the company of the authenticated user.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The collaborators ordered by name.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UserResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var actor = HttpContext.GetAuthenticatedUser();
        var users = await _queries.Handle(new GetUsersByCompanyIdQuery(actor.CompanyId), cancellationToken);
        return Ok(UserResourceFromEntityAssembler.ToResourcesFromEntities(users));
    }

    /// <summary>
    /// Returns a collaborator of the company of the authenticated user.
    /// </summary>
    /// <param name="userId">Identifier of the collaborator.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The collaborator.</returns>
    [HttpGet("{userId}")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(string userId, CancellationToken cancellationToken)
    {
        var actor = HttpContext.GetAuthenticatedUser();
        var user = await _queries.Handle(new GetUserByIdQuery(userId), cancellationToken);
        if (user == null || user.CompanyId != actor.CompanyId)
        {
            throw new IamRuleException("USER_NOT_FOUND", "The collaborator was not found.", DomainErrorKind.NotFound);
        }

        return Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(user));
    }

    /// <summary>
    /// Grants platform access to a collaborator.
    /// </summary>
    /// <param name="userId">Identifier of the collaborator.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The updated collaborator.</returns>
    [HttpPost("{userId}/grant")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> GrantAccess(string userId, CancellationToken cancellationToken)
    {
        var actor = HttpContext.GetAuthenticatedUser();
        var user = await _commands.Handle(new GrantUserAccessCommand(actor.Id, userId), cancellationToken);
        return Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(user));
    }

    /// <summary>
    /// Revokes the platform access of a collaborator.
    /// </summary>
    /// <param name="userId">Identifier of the collaborator.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The updated collaborator.</returns>
    [HttpPost("{userId}/revoke")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RevokeAccess(string userId, CancellationToken cancellationToken)
    {
        var actor = HttpContext.GetAuthenticatedUser();
        var user = await _commands.Handle(new RevokeUserAccessCommand(actor.Id, userId), cancellationToken);
        return Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(user));
    }

    /// <summary>
    /// Assigns a role to a collaborator and resets its permissions to the role defaults.
    /// </summary>
    /// <param name="userId">Identifier of the collaborator.</param>
    /// <param name="resource">The role to assign.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The updated collaborator.</returns>
    [HttpPut("{userId}/role")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AssignRole(
        string userId,
        [FromBody] AssignRoleResource resource,
        CancellationToken cancellationToken)
    {
        var actor = HttpContext.GetAuthenticatedUser();
        var command = IamCommandFromResourceAssembler.ToCommandFromResource(actor.Id, userId, resource);
        var user = await _commands.Handle(command, cancellationToken);
        return Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(user));
    }

    /// <summary>
    /// Replaces the permissions of a collaborator.
    /// </summary>
    /// <param name="userId">Identifier of the collaborator.</param>
    /// <param name="resource">The new permissions.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The updated collaborator.</returns>
    [HttpPut("{userId}/permissions")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdatePermissions(
        string userId,
        [FromBody] UpdatePermissionsResource resource,
        CancellationToken cancellationToken)
    {
        var actor = HttpContext.GetAuthenticatedUser();
        var command = IamCommandFromResourceAssembler.ToCommandFromResource(actor.Id, userId, resource);
        var user = await _commands.Handle(command, cancellationToken);
        return Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(user));
    }
}
