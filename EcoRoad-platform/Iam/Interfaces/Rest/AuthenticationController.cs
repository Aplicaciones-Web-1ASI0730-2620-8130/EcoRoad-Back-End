using EcoRoad.Iam.Application.CommandServices;
using EcoRoad.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using EcoRoad.Iam.Infrastructure.Pipeline.Middleware.Extensions;
using EcoRoad.Iam.Interfaces.Rest.Resources;
using EcoRoad.Iam.Interfaces.Rest.Transform;
using EcoRoad.Shared.Interfaces.Rest.Resources;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoad.Iam.Interfaces.Rest;

/// <summary>
/// Endpoints to register company administrators and to open, read and close sessions.
/// </summary>
[ApiController]
[Route("api/iam")]
[Produces("application/json")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserCommandService _commands;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationController" /> class.
    /// </summary>
    /// <param name="commands">Command service of users.</param>
    public AuthenticationController(IUserCommandService commands)
    {
        _commands = commands;
    }

    /// <summary>
    /// Registers the first administrator of a newly onboarded company.
    /// </summary>
    /// <param name="resource">Administrator data.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The new administrator.</returns>
    [HttpPost("sign-up")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> SignUp([FromBody] SignUpResource resource, CancellationToken cancellationToken)
    {
        var user = await _commands.Handle(IamCommandFromResourceAssembler.ToCommandFromResource(resource), cancellationToken);
        return Created($"/api/iam/users/{user.Id}", UserResourceFromEntityAssembler.ToResourceFromEntity(user));
    }

    /// <summary>
    /// Opens a session and returns a bearer token.
    /// </summary>
    /// <param name="resource">Credentials of the collaborator.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The session token and the authenticated collaborator.</returns>
    [HttpPost("sessions")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthenticatedUserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> SignIn([FromBody] SignInResource resource, CancellationToken cancellationToken)
    {
        var session = await _commands.Handle(IamCommandFromResourceAssembler.ToCommandFromResource(resource), cancellationToken);
        return Ok(AuthenticatedUserResourceFromSessionAssembler.ToResourceFromSession(session));
    }

    /// <summary>
    /// Returns the collaborator of the current session.
    /// </summary>
    /// <returns>The authenticated collaborator.</returns>
    [HttpGet("sessions/current")]
    [Authorize]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status401Unauthorized)]
    public IActionResult GetCurrentSession()
    {
        return Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(HttpContext.GetAuthenticatedUser()));
    }

    /// <summary>
    /// Ends the current session. Tokens are stateless, so the client must discard its token.
    /// </summary>
    /// <returns>No content.</returns>
    [HttpDelete("sessions/current")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status401Unauthorized)]
    public IActionResult EndSession()
    {
        return NoContent();
    }
}
