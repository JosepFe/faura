namespace Template.Sample.Application.DTOs;

using Template.Sample.Domain.Enums;

/// <summary>
/// Data transfer object for SampleItem.
/// </summary>
/// <param name="Id">The unique identifier.</param>
/// <param name="Name">The name of the item.</param>
/// <param name="Description">The description of the item.</param>
/// <param name="Category">The category of the item.</param>
/// <param name="Status">The status of the item.</param>
/// <param name="Tags">The tags associated with the item.</param>
/// <param name="IsActive">Indicates if the item is active.</param>
/// <param name="CreatedByUserId">The ID of the user who created the item.</param>
/// <param name="CreatedAt">The creation timestamp.</param>
/// <param name="UpdatedAt">The last update timestamp.</param>
public record SampleItemDto(
    string Id,
    string Name,
    string? Description,
    SampleCategory Category,
    SampleStatus Status,
    List<string> Tags,
    bool IsActive,
    string? CreatedByUserId,
    DateTime CreatedAt,
    DateTime UpdatedAt);
