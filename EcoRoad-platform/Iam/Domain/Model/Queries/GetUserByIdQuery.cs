namespace EcoRoad.Iam.Domain.Model.Queries;

/// <summary>
/// Retrieves a user by its identifier.
/// </summary>
/// <param name="UserId">Identifier of the user.</param>
public record GetUserByIdQuery(string UserId);
