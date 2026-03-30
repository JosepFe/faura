namespace Template.Api.Contracts.Sample.SampleItems;

using Template.Sample.Domain.Enums;

/// <summary>
/// Response with sample item details.
/// </summary>
/// <param name="Id">The unique identifier.</param>
/// <param name="Name">The name of the item.</param>
/// <param name="Description">The description.</param>
/// <param name="Category">The category.</param>
/// <param name="Status">The status.</param>
/// <param name="Tags">The tags.</param>
/// <param name="IsActive">Indicates if the item is active.</param>
/// <param name="CreatedByUserId">The ID of the user who created the item.</param>
/// <param name="CreatedAt">The creation timestamp.</param>
/// <param name="UpdatedAt">The last update timestamp.</param>
public record GetSampleItemByIdResponse(
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
