namespace Template.Sample.Domain.Repositories;

using Template.Sample.Domain.Entities;
using Template.Sample.Domain.Enums;
using Template.Shared.Domain.Models;

/// <summary>
/// Repository interface for SampleItem operations.
/// </summary>
public interface ISampleItemRepository
{
    /// <summary>
    /// Gets a sample item by its ID.
    /// </summary>
    /// <param name="id">The item ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The sample item if found, null otherwise.</returns>
    Task<SampleItem?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all sample items with pagination and filtering.
    /// </summary>
    /// <param name="pageNumber">The page number (1-based).</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="searchTerm">Optional search term to filter by name.</param>
    /// <param name="category">Optional category filter.</param>
    /// <param name="status">Optional status filter.</param>
    /// <param name="isActive">Optional active status filter.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated result of sample items.</returns>
    Task<PagedResult<SampleItem>> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        SampleCategory? category = null,
        SampleStatus? status = null,
        bool? isActive = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new sample item.
    /// </summary>
    /// <param name="sampleItem">The sample item to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AddAsync(SampleItem sampleItem, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing sample item.
    /// </summary>
    /// <param name="sampleItem">The sample item to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task UpdateAsync(SampleItem sampleItem, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a sample item.
    /// </summary>
    /// <param name="id">The ID of the item to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a sample item with the given name exists.
    /// </summary>
    /// <param name="name">The name to check.</param>
    /// <param name="excludeId">Optional ID to exclude from the check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if exists, false otherwise.</returns>
    Task<bool> ExistsByNameAsync(string name, string? excludeId = null, CancellationToken cancellationToken = default);
}
