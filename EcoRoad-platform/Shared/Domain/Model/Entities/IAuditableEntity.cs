namespace EcoRoad.Shared.Domain.Model.Entities;

/// <summary>
/// Marks an entity whose audit timestamps are maintained by the persistence layer.
/// </summary>
/// <remarks>
/// <see cref="CreatedAt" /> is set once when the entity is inserted and
/// <see cref="UpdatedAt" /> is refreshed on every save. Both values are stored in UTC.
/// </remarks>
public interface IAuditableEntity
{
    /// <summary>
    /// Gets or sets the UTC date and time when the entity was first persisted.
    /// </summary>
    DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the UTC date and time when the entity was last persisted.
    /// </summary>
    DateTime? UpdatedAt { get; set; }
}
