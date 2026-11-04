namespace EcoRoad.Iam.Domain.Model.Commands;

/// <summary>
/// Authenticates a collaborator and opens a session.
/// </summary>
/// <param name="CompanyId">Identifier of the company; optional when the email is unique across companies.</param>
/// <param name="Email">Email address of the collaborator.</param>
/// <param name="Password">Plain text password.</param>
public record SignInCommand(string? CompanyId, string Email, string Password);
