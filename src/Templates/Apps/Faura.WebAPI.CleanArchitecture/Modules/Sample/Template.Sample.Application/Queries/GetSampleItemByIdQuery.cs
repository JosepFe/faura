namespace Template.Sample.Application.Queries;

using Faura.Infrastructure.Result;
using Template.Sample.Application.DTOs;
using Template.Shared.Application.SimpleMediator;

/// <summary>
/// Query to get a sample item by its ID.
/// </summary>
/// <param name="Id">The unique identifier of the sample item.</param>
public record GetSampleItemByIdQuery(string Id) : IRequest<FauraResult<SampleItemDto>>;
