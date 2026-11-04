namespace EcoRoad.Iam.Domain.Model.Commands;

/// <summary>
/// Registers the first administrator of a company that has just been onboarded.
/// </summary>
/// <param name="CompanyId">Identifier of the company account.</param>
/// <param name="Name">Full name of the administrator.</param>
/// <param name="Email">Email address used to sign in.</param>
/// <param name="Password">Plain text password; it is hashed before being stored.</param>
public record SignUpCompanyAdministratorCommand(string CompanyId, string Name, string Email, string Password);
