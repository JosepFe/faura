namespace Template.Sample.Application.Handlers;

using Faura.Infrastructure.Logger.Extensions;
using Faura.Infrastructure.Result;
using Microsoft.Extensions.Logging;
using Template.Sample.Application.DTOs;
using Template.Sample.Application.Queries;
using Template.Sample.Domain.Repositories;
using Template.Shared.Application.SimpleMediator;

/// <summary>
/// Handler for getting a sample item by ID.
/// </summary>
/// <param name="repository">The sample item repository.</param>
/// <param name="logger">The logger.</param>
public class GetSampleItemByIdHandler(
    ISampleItemRepository repository,
    ILogger<GetSampleItemByIdHandler> logger) : IRequestHandler<GetSampleItemByIdQuery, FauraResult<SampleItemDto>>
{

    /// <inheritdoc/>
    public async Task<FauraResult<SampleItemDto>> Handle(
        GetSampleItemByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        logger.LogFauraInformation("Getting sample item by ID: {Id}", query.Id);

        var sampleItem = await repository.GetByIdAsync(query.Id, cancellationToken);

        if (sampleItem == null)
        {
            return FauraResult<SampleItemDto>.Error(
                new FauraError("SAMPLE_ITEM_NOT_FOUND", $"Sample item with ID '{query.Id}' not found"));
        }

        var dto = new SampleItemDto(
            sampleItem.Id,
            sampleItem.Name,
            sampleItem.Description,
            sampleItem.Category,
            sampleItem.Status,
            sampleItem.Tags,
            sampleItem.IsActive,
            sampleItem.CreatedByUserId,
            sampleItem.CreatedAt,
            sampleItem.UpdatedAt);

        return FauraResult<SampleItemDto>.Success(dto);
    }
}
