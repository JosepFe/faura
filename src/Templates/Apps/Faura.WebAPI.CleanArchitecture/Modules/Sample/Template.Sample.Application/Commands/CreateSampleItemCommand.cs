namespace Template.Sample.Application.Commands;

using Faura.Infrastructure.Result;
using Template.Sample.Application.DTOs;
using Template.Sample.Domain.Enums;
using Template.Shared.Application.SimpleMediator;

/// <summary>
/// Command to create a new sample item.
/// </summary>
/// <param name="Name">The name of the item.</param>
/// <param name="Description">Optional description.</param>
/// <param name="Category">The category of the item.</param>
/// <param name="Tags">Optional tags.</param>
/// <param name="CreatedByUserId">Optional user ID who created the item.</param>
public record CreateSampleItemCommand(
    string Name,
    string? Description,
    SampleCategory Category,
    IEnumerable<string>? Tags,
    string? CreatedByUserId) : IRequest<FauraResult<SampleItemDto>>;
