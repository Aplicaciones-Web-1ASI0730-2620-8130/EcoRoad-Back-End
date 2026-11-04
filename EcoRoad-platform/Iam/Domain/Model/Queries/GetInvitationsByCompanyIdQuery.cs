using EcoRoad.Iam.Domain.Model.ValueObjects;

namespace EcoRoad.Iam.Domain.Model.Queries;

/// <summary>
/// Lists the invitations sent by a company, optionally filtered by status.
/// </summary>
/// <param name="CompanyId">Identifier of the company.</param>
/// <param name="Status">Optional status filter.</param>
public record GetInvitationsByCompanyIdQuery(string CompanyId, InvitationStatus? Status);
