namespace Template.Sample.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Template.Sample.Domain.Entities;
using Template.Sample.Domain.Enums;
using Template.Sample.Domain.Repositories;
using Template.Sample.Infrastructure.Persistence;
using Template.Shared.Domain.Models;

/// <summary>
/// Repository implementation for SampleItem.
/// </summary>
/// <param name="context">The database context.</param>
public class SampleItemRepository(SampleDbContext context) : ISampleItemRepository
{

    /// <inheritdoc/>
    public async Task<SampleItem?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await context.SampleItems
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<PagedResult<SampleItem>> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        SampleCategory? category = null,
        SampleStatus? status = null,
        bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.SampleItems.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x => x.Name.Contains(searchTerm));
        }

        if (category.HasValue)
        {
            query = query.Where(x => x.Category == category.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status.Value);
        }

        if (isActive.HasValue)
        {
            query = query.Where(x => x.IsActive == isActive.Value);
        }

        // Get total count
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination
        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<SampleItem>(items, totalCount, pageNumber, pageSize);
    }

    /// <inheritdoc/>
    public async Task AddAsync(SampleItem sampleItem, CancellationToken cancellationToken = default)
    {
        await context.SampleItems.AddAsync(sampleItem, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(SampleItem sampleItem, CancellationToken cancellationToken = default)
    {
        context.SampleItems.Update(sampleItem);
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var item = await GetByIdAsync(id, cancellationToken);
        if (item != null)
        {
            context.SampleItems.Remove(item);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsByNameAsync(string name, string? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = context.SampleItems.Where(x => x.Name == name);

        if (!string.IsNullOrEmpty(excludeId))
        {
            query = query.Where(x => x.Id != excludeId);
        }

        return await query.AnyAsync(cancellationToken);
    }
}
