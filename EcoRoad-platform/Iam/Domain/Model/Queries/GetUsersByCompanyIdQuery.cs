namespace EcoRoad.Iam.Domain.Model.Queries;

/// <summary>
/// Lists the collaborators of a company.
/// </summary>
/// <param name="CompanyId">Identifier of the company.</param>
public record GetUsersByCompanyIdQuery(string CompanyId);
