namespace Template.Api.Controllers.Sample;

using System.Net;
using Faura.Infrastructure.Result;
using Microsoft.AspNetCore.Mvc;
using Template.Api.Contracts.Sample.SampleItems;
using Template.Api.Mapping.Sample;
using Template.Sample.Application.Commands;
using Template.Sample.Application.Queries;
using Template.Shared.Application.SimpleMediator;

/// <summary>
/// Controller for managing sample items.
/// </summary>
/// <param name="mediator">The mediator.</param>
[ApiController]
[Route("api/sample-items")]
[Consumes("application/json")]
[Produces("application/json")]
public class SampleItemsController(IMediator mediator) : ControllerBase
{

    /// <summary>
    /// Create a new sample item.
    /// </summary>
    /// <param name="request">Sample item creation data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created sample item.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateSampleItemResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateSampleItem(
        [FromBody] CreateSampleItemRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request.ToCreateCommand(), cancellationToken);

        if (!result.IsSuccess)
        {
            return result.BuildResult();
        }

        var response = result.Data.ToCreateResponse();
        return FauraResult<CreateSampleItemResponse>.Success(response).BuildResult(HttpStatusCode.Created);
    }

    /// <summary>
    /// Get sample item by ID.
    /// </summary>
    /// <param name="id">The unique identifier of the sample item.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The sample item details.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetSampleItemByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSampleItemById(
        [FromRoute] string id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetSampleItemByIdQuery(id), cancellationToken);

        if (!result.IsSuccess)
        {
            return result.BuildResult();
        }

        var response = result.Data.ToGetByIdResponse();
        return FauraResult<GetSampleItemByIdResponse>.Success(response).BuildResult();
    }

    /// <summary>
    /// Get all sample items with pagination and filters.
    /// </summary>
    /// <param name="request">The pagination and filter parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of sample items.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(GetSampleItemsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSampleItems(
        [FromQuery] GetSampleItemsRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request.ToQuery(), cancellationToken);

        if (!result.IsSuccess)
        {
            return result.BuildResult();
        }

        var response = result.Data.ToGetAllResponse();
        return FauraResult<GetSampleItemsResponse>.Success(response).BuildResult();
    }

    /// <summary>
    /// Update an existing sample item.
    /// </summary>
    /// <param name="id">The unique identifier of the sample item.</param>
    /// <param name="request">Sample item update data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated sample item.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(GetSampleItemByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSampleItem(
        [FromRoute] string id,
        [FromBody] UpdateSampleItemRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request.ToUpdateCommand(id), cancellationToken);

        if (!result.IsSuccess)
        {
            return result.BuildResult();
        }

        var response = result.Data.ToGetByIdResponse();
        return FauraResult<GetSampleItemByIdResponse>.Success(response).BuildResult();
    }

    /// <summary>
    /// Delete a sample item.
    /// </summary>
    /// <param name="id">The unique identifier of the sample item.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content on success.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSampleItem(
        [FromRoute] string id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteSampleItemCommand(id), cancellationToken);

        if (!result.IsSuccess)
        {
            return result.BuildResult();
        }

        return NoContent();
    }
}
