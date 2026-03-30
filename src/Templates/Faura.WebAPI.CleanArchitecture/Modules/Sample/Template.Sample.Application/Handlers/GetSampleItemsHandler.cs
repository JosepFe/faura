namespace Template.Sample.Application.Handlers;

using Faura.Infrastructure.Logger.Extensions;
using Faura.Infrastructure.Result;
using Microsoft.Extensions.Logging;
using Template.Sample.Application.DTOs;
using Template.Sample.Application.Queries;
using Template.Sample.Domain.Repositories;
using Template.Shared.Application.SimpleMediator;
using Template.Shared.Domain.Models;

/// <summary>
/// Handler for getting all sample items with pagination.
/// </summary>
/// <param name="repository">The sample item repository.</param>
/// <param name="logger">The logger.</param>
public class GetSampleItemsHandler(
    ISampleItemRepository repository,
    ILogger<GetSampleItemsHandler> logger) : IRequestHandler<GetSampleItemsQuery, FauraResult<PagedResult<SampleItemDto>>>
{

    /// <inheritdoc/>
    public async Task<FauraResult<PagedResult<SampleItemDto>>> Handle(
        GetSampleItemsQuery query,
        CancellationToken cancellationToken = default)
    {
        logger.LogFauraInformation(
            "Getting sample items - Page: {PageNumber}, Size: {PageSize}",
            query.PageNumber.ToString(),
            query.PageSize.ToString());

        var pagedItems = await repository.GetAllAsync(
            query.PageNumber,
            query.PageSize,
            query.SearchTerm,
            query.Category,
            query.Status,
            query.IsActive,
            cancellationToken);

        var dtos = pagedItems.Items.Select(item => new SampleItemDto(
            item.Id,
            item.Name,
            item.Description,
            item.Category,
            item.Status,
            item.Tags,
            item.IsActive,
            item.CreatedByUserId,
            item.CreatedAt,
            item.UpdatedAt));

        var result = new PagedResult<SampleItemDto>(
            dtos,
            pagedItems.TotalCount,
            pagedItems.PageNumber,
            pagedItems.PageSize);

        return FauraResult<PagedResult<SampleItemDto>>.Success(result);
    }
}
