namespace Template.Sample.Application.Commands;

using Faura.Infrastructure.Result;
using Template.Shared.Application.SimpleMediator;

/// <summary>
/// Command to delete a sample item.
/// </summary>
/// <param name="Id">The ID of the item to delete.</param>
public record DeleteSampleItemCommand(string Id) : IRequest<FauraResult<bool>>;
