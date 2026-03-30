namespace Template.Api.Contracts.Sample.SampleItems;

using System.Text.Json.Serialization;
using Template.Sample.Domain.Enums;

/// <summary>
/// Request to create a new sample item.
/// </summary>
/// <param name="Name">The name of the item.</param>
/// <param name="Description">Optional description.</param>
/// <param name="Category">The category of the item.</param>
/// <param name="Tags">Optional tags.</param>
/// <param name="CreatedByUserId">Optional user ID who created the item.</param>
public record CreateSampleItemRequest(
    string Name,
    string? Description,
    [property: JsonRequired] SampleCategory Category,
    List<string>? Tags,
    string? CreatedByUserId);
