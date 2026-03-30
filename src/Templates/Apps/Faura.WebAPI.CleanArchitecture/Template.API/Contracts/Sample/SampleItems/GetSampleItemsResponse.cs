namespace Template.Api.Contracts.Sample.SampleItems;

using Template.Sample.Domain.Enums;

/// <summary>
/// Item in the sample items list.
/// </summary>
/// <param name="Id">The unique identifier.</param>
/// <param name="Name">The name of the item.</param>
/// <param name="Description">The description.</param>
/// <param name="Category">The category.</param>
/// <param name="Status">The status.</param>
/// <param name="IsActive">Indicates if the item is active.</param>
/// <param name="CreatedAt">The creation timestamp.</param>
public record SampleItemItem(
    string Id,
    string Name,
    string? Description,
    SampleCategory Category,
    SampleStatus Status,
    bool IsActive,
    DateTime CreatedAt);

/// <summary>
/// Response with paginated list of sample items.
/// </summary>
/// <param name="Items">The sample items in the current page.</param>
/// <param name="TotalCount">The total count of items across all pages.</param>
/// <param name="PageNumber">The current page number (1-based).</param>
/// <param name="PageSize">The number of items per page.</param>
/// <param name="TotalPages">The total number of pages.</param>
/// <param name="HasPreviousPage">Indicates if there is a previous page.</param>
/// <param name="HasNextPage">Indicates if there is a next page.</param>
public record GetSampleItemsResponse(
    List<SampleItemItem> Items,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage);

