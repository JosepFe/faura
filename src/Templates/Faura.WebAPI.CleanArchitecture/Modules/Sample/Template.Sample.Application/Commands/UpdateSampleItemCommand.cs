namespace Template.Sample.Application.Commands;

using Faura.Infrastructure.Result;
using Template.Sample.Application.DTOs;
using Template.Sample.Domain.Enums;
using Template.Shared.Application.SimpleMediator;

/// <summary>
/// Command to update an existing sample item.
/// </summary>
/// <param name="Id">The ID of the item to update.</param>
/// <param name="Name">The new name.</param>
/// <param name="Description">The new description.</param>
/// <param name="Category">The new category.</param>
/// <param name="Status">The new status.</param>
/// <param name="Tags">The new tags.</param>
public record UpdateSampleItemCommand(
    string Id,
    string Name,
    string? Description,
    SampleCategory Category,
    SampleStatus Status,
    IEnumerable<string>? Tags) : IRequest<FauraResult<SampleItemDto>>;
