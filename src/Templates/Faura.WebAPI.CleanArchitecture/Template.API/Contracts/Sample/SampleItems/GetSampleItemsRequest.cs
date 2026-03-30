namespace Template.Api.Contracts.Sample.SampleItems;

using Template.Sample.Domain.Enums;

/// <summary>
/// Request to get all sample items with pagination and filters.
/// </summary>
/// <param name="PageNumber">The page number (1-based).</param>
/// <param name="PageSize">The page size.</param>
/// <param name="SearchTerm">Optional search term to filter by name.</param>
/// <param name="Category">Optional category filter.</param>
/// <param name="Status">Optional status filter.</param>
/// <param name="IsActive">Optional active status filter.</param>
public record GetSampleItemsRequest(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    SampleCategory? Category = null,
    SampleStatus? Status = null,
    bool? IsActive = null);
