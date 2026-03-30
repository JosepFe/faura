namespace Template.Sample.Domain.Entities;

using Template.Sample.Domain.Enums;
using Template.Shared.Domain.Entities;

/// <summary>
/// Represents a sample item entity.
/// This is an example entity demonstrating Clean Architecture patterns.
/// </summary>
public class SampleItem : EntityBase
{
    /// <summary>
    /// MongoDB collection name for this entity.
    /// </summary>
    public static readonly string CollectionName = "sample_items";

    // Private constructor to enforce factory method pattern
    private SampleItem()
        : base()
    {
    }

    /// <summary>
    /// Gets the name of the sample item.
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// Gets the description of the sample item.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets the category of the sample item.
    /// </summary>
    public SampleCategory Category { get; private set; }

    /// <summary>
    /// Gets the status of the sample item.
    /// </summary>
    public SampleStatus Status { get; private set; }

    /// <summary>
    /// Gets the tags associated with the sample item.
    /// </summary>
    public List<string> Tags { get; private set; } = [];

    /// <summary>
    /// Gets a value indicating whether the item is active.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Gets the ID of the user who created this item.
    /// </summary>
    public string? CreatedByUserId { get; private set; }

    /// <summary>
    /// Factory method to create a new sample item.
    /// </summary>
    /// <param name="name">The name of the item.</param>
    /// <param name="category">The category of the item.</param>
    /// <param name="description">Optional description.</param>
    /// <param name="createdByUserId">Optional user ID who created the item.</param>
    /// <returns>A new SampleItem instance.</returns>
    /// <exception cref="ArgumentException">Thrown when name is null or empty.</exception>
    public static SampleItem Create(
        string name,
        SampleCategory category,
        string? description = null,
        string? createdByUserId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty", nameof(name));
        }

        return new SampleItem
        {
            Name = name,
            Description = description,
            Category = category,
            Status = SampleStatus.Draft,
            CreatedByUserId = createdByUserId,
            IsActive = true,
        };
    }

    /// <summary>
    /// Updates the item's name and description.
    /// </summary>
    /// <param name="name">The new name.</param>
    /// <param name="description">The new description.</param>
    /// <exception cref="ArgumentException">Thrown when name is null or empty.</exception>
    public void Update(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty", nameof(name));
        }

        Name = name;
        Description = description;
        MarkAsUpdated();
    }

    /// <summary>
    /// Sets the tags for the item.
    /// </summary>
    /// <param name="tags">The tags to set.</param>
    public void SetTags(IEnumerable<string> tags)
    {
        Tags = tags?.ToList() ?? new List<string>();
        MarkAsUpdated();
    }

    /// <summary>
    /// Changes the category of the item.
    /// </summary>
    /// <param name="category">The new category.</param>
    public void ChangeCategory(SampleCategory category)
    {
        Category = category;
        MarkAsUpdated();
    }

    /// <summary>
    /// Changes the status of the item.
    /// </summary>
    /// <param name="status">The new status.</param>
    public void ChangeStatus(SampleStatus status)
    {
        Status = status;
        MarkAsUpdated();
    }

    /// <summary>
    /// Activates the item.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        MarkAsUpdated();
    }

    /// <summary>
    /// Deactivates the item.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        MarkAsUpdated();
    }

    /// <summary>
    /// Archives the item by setting its status to Archived and deactivating it.
    /// </summary>
    public void Archive()
    {
        Status = SampleStatus.Archived;
        IsActive = false;
        MarkAsUpdated();
    }
}
