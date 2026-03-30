namespace Template.Sample.Application.Queries;

using Faura.Infrastructure.Result;
using Template.Sample.Application.DTOs;
using Template.Sample.Domain.Enums;
using Template.Shared.Application.SimpleMediator;
using Template.Shared.Domain.Models;

/// <summary>
/// Query to get all sample items with pagination and filtering.
/// </summary>
/// <param name="PageNumber">The page number (1-based).</param>
/// <param name="PageSize">The page size.</param>
/// <param name="SearchTerm">Optional search term to filter by name.</param>
/// <param name="Category">Optional category filter.</param>
/// <param name="Status">Optional status filter.</param>
/// <param name="IsActive">Optional active status filter.</param>
public record GetSampleItemsQuery(
    int PageNumber,
    int PageSize,
    string? SearchTerm = null,
    SampleCategory? Category = null,
    SampleStatus? Status = null,
    bool? IsActive = null) : IRequest<FauraResult<PagedResult<SampleItemDto>>>;
