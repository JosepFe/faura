namespace Template.Shared.Domain.Entities;

/// <summary>
/// Base class for all entities in the system.
/// Provides common properties like Id, CreatedAt, and UpdatedAt.
/// </summary>
public abstract class EntityBase
{
    /// <summary>
    /// Gets the unique identifier of the entity.
    /// </summary>
    public string Id { get; protected set; } = null!;

    /// <summary>
    /// Gets the date and time when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; protected set; }

    /// <summary>
    /// Gets the date and time when the entity was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; protected set; }

    /// <summary>
    /// Marks the entity as updated by setting UpdatedAt to current UTC time.
    /// </summary>
    public void MarkAsUpdated() => UpdatedAt = DateTime.UtcNow;

    /// <summary>
    /// Sets the entity ID. Should only be called by infrastructure layer.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    public void SetId(string id) => Id = id;

    /// <summary>
    /// Sets the CreatedAt timestamp. Should only be called by infrastructure layer.
    /// </summary>
    /// <param name="createdAt">The creation timestamp.</param>
    public void SetCreatedAt(DateTime createdAt) => CreatedAt = createdAt;

    /// <summary>
    /// Sets the UpdatedAt timestamp. Should only be called by infrastructure layer.
    /// </summary>
    /// <param name="updatedAt">The update timestamp.</param>
    public void SetUpdatedAt(DateTime updatedAt) => UpdatedAt = updatedAt;
}
