namespace Template.Api.Contracts.Sample.SampleItems;

using System.Text.Json.Serialization;
using Template.Sample.Domain.Enums;

/// <summary>
/// Request to update an existing sample item.
/// </summary>
/// <param name="Name">The new name.</param>
/// <param name="Description">The new description.</param>
/// <param name="Category">The new category.</param>
/// <param name="Status">The new status.</param>
/// <param name="Tags">The new tags.</param>
public record UpdateSampleItemRequest(
    string Name,
    string? Description,
    [property: JsonRequired] SampleCategory Category,
    [property: JsonRequired] SampleStatus Status,
    List<string>? Tags);
