namespace Template.Api.Mapping.Sample;

using Template.Api.Contracts.Sample.SampleItems;
using Template.Sample.Application.Commands;
using Template.Sample.Application.DTOs;
using Template.Sample.Application.Queries;
using Template.Shared.Domain.Models;

/// <summary>
/// Mapping extensions for Sample module.
/// </summary>
public static class SampleItemMappingExtensions
{
    /// <summary>
    /// Maps CreateSampleItemRequest to CreateSampleItemCommand.
    /// </summary>
    public static CreateSampleItemCommand ToCreateCommand(this CreateSampleItemRequest request)
        => new CreateSampleItemCommand(
            request.Name,
            request.Description,
            request.Category,
            request.Tags,
            request.CreatedByUserId);

    /// <summary>
    /// Maps UpdateSampleItemRequest to UpdateSampleItemCommand.
    /// </summary>
    public static UpdateSampleItemCommand ToUpdateCommand(this UpdateSampleItemRequest request, string id)
        => new UpdateSampleItemCommand(
            id,
            request.Name,
            request.Description,
            request.Category,
            request.Status,
            request.Tags);

    /// <summary>
    /// Maps GetSampleItemsRequest to GetSampleItemsQuery.
    /// </summary>
    public static GetSampleItemsQuery ToQuery(this GetSampleItemsRequest request)
        => new GetSampleItemsQuery(
            request.PageNumber,
            request.PageSize,
            request.SearchTerm,
            request.Category,
            request.Status,
            request.IsActive);

    /// <summary>
    /// Maps SampleItemDto to CreateSampleItemResponse.
    /// </summary>
    public static CreateSampleItemResponse ToCreateResponse(this SampleItemDto dto)
        => new CreateSampleItemResponse(
            dto.Id,
            dto.Name,
            dto.Category,
            dto.Status,
            dto.CreatedAt);

    /// <summary>
    /// Maps SampleItemDto to GetSampleItemByIdResponse.
    /// </summary>
    public static GetSampleItemByIdResponse ToGetByIdResponse(this SampleItemDto dto)
        => new GetSampleItemByIdResponse(
            dto.Id,
            dto.Name,
            dto.Description,
            dto.Category,
            dto.Status,
            dto.Tags,
            dto.IsActive,
            dto.CreatedByUserId,
            dto.CreatedAt,
            dto.UpdatedAt);

    /// <summary>
    /// Maps PagedResult of SampleItemDto to GetSampleItemsResponse.
    /// </summary>
    public static GetSampleItemsResponse ToGetAllResponse(this PagedResult<SampleItemDto> pagedResult)
    {
        var items = pagedResult.Items.Select(dto => new SampleItemItem(
            dto.Id,
            dto.Name,
            dto.Description,
            dto.Category,
            dto.Status,
            dto.IsActive,
            dto.CreatedAt)).ToList();

        return new GetSampleItemsResponse(
            items,
            pagedResult.TotalCount,
            pagedResult.PageNumber,
            pagedResult.PageSize,
            pagedResult.TotalPages,
            pagedResult.HasPreviousPage,
            pagedResult.HasNextPage);
    }
}
