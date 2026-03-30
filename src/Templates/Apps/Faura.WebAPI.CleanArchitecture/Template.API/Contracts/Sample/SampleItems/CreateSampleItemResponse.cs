namespace Template.Api.Contracts.Sample.SampleItems;

using Template.Sample.Domain.Enums;

/// <summary>
/// Response after creating a sample item.
/// </summary>
/// <param name="Id">The unique identifier.</param>
/// <param name="Name">The name of the item.</param>
/// <param name="Category">The category.</param>
/// <param name="Status">The status.</param>
/// <param name="CreatedAt">The creation timestamp.</param>
public record CreateSampleItemResponse(
    string Id,
    string Name,
    SampleCategory Category,
    SampleStatus Status,
    DateTime CreatedAt);
