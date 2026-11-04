using EcoRoad.Iam.Application.CommandServices;
using EcoRoad.Iam.Application.QueryServices;
using EcoRoad.Iam.Domain.Model;
using EcoRoad.Iam.Domain.Model.Queries;
using EcoRoad.Iam.Domain.Model.ValueObjects;
using EcoRoad.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using EcoRoad.Iam.Infrastructure.Pipeline.Middleware.Extensions;
using EcoRoad.Iam.Interfaces.Rest.Resources;
using EcoRoad.Iam.Interfaces.Rest.Transform;
using EcoRoad.Shared.Domain.Model.Exceptions;
using EcoRoad.Shared.Interfaces.Rest.Resources;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoad.Iam.Interfaces.Rest;

/// <summary>
/// Endpoints to invite collaborators and to accept invitations.
/// </summary>
[ApiController]
[Route("api/iam/invitations")]
[Produces("application/json")]
[Authorize]
public class InvitationsController : ControllerBase
{
    private readonly IUserInvitationCommandService _commands;
    private readonly IUserInvitationQueryService _queries;

    /// <summary>
    /// Initializes a new instance of the <see cref="InvitationsController" /> class.
    /// </summary>
    /// <param name="commands">Command service of invitations.</param>
    /// <param name="queries">Query service of invitations.</param>
    public InvitationsController(IUserInvitationCommandService commands, IUserInvitationQueryService queries)
    {
        _commands = commands;
        _queries = queries;
    }

    /// <summary>
    /// Lists the invitations sent by the company of the authenticated user.
    /// </summary>
    /// <param name="status">Optional status filter.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The invitations, newest first.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<InvitationResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInvitations(
        [FromQuery] InvitationStatus? status,
        CancellationToken cancellationToken)
    {
        var actor = HttpContext.GetAuthenticatedUser();
        var invitations = await _queries.Handle(
            new GetInvitationsByCompanyIdQuery(actor.CompanyId, status),
            cancellationToken);
        return Ok(InvitationResourceFromEntityAssembler.ToResourcesFromEntities(invitations));
    }

    /// <summary>
    /// Returns an invitation so the invited person can review it before accepting.
    /// </summary>
    /// <param name="invitationId">Identifier of the invitation.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The invitation.</returns>
    [HttpGet("{invitationId}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(InvitationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInvitation(string invitationId, CancellationToken cancellationToken)
    {
        var invitation = await _queries.Handle(new GetInvitationByIdQuery(invitationId), cancellationToken)
            ?? throw new IamRuleException("INVITATION_NOT_FOUND", "The invitation was not found.", DomainErrorKind.NotFound);
        return Ok(InvitationResourceFromEntityAssembler.ToResourceFromEntity(invitation));
    }

    /// <summary>
    /// Invites a new collaborator to the company of the authenticated administrator.
    /// </summary>
    /// <param name="resource">Data of the invited person.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The pending invitation.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(InvitationResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> InviteUser([FromBody] InviteUserResource resource, CancellationToken cancellationToken)
    {
        var actor = HttpContext.GetAuthenticatedUser();
        var command = IamCommandFromResourceAssembler.ToCommandFromResource(actor.Id, resource);
        var invitation = await _commands.Handle(command, cancellationToken);
        return CreatedAtAction(
            nameof(GetInvitation),
            new { invitationId = invitation.Id },
            InvitationResourceFromEntityAssembler.ToResourceFromEntity(invitation));
    }

    /// <summary>
    /// Accepts an invitation. The account is created with pending access until an administrator grants it.
    /// </summary>
    /// <param name="invitationId">Identifier of the invitation.</param>
    /// <param name="resource">Password chosen by the invited person.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The created collaborator.</returns>
    [HttpPost("{invitationId}/acceptance")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AcceptInvitation(
        string invitationId,
        [FromBody] AcceptInvitationResource resource,
        CancellationToken cancellationToken)
    {
        var command = IamCommandFromResourceAssembler.ToCommandFromResource(invitationId, resource);
        var user = await _commands.Handle(command, cancellationToken);
        return Created($"/api/iam/users/{user.Id}", UserResourceFromEntityAssembler.ToResourceFromEntity(user));
    }
}
