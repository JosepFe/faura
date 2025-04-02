namespace Faura.Infrastructure.UnitOfWork.Repositories;

using Faura.Infrastructure.UnitOfWork.Enums;
using Faura.Infrastructure.UnitOfWork.Exceptions;
using Faura.Infrastructure.UnitOfWork.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

/// <summary>
/// Generic repository implementation for data access operations
/// </summary>
/// <typeparam name="TEntity">The entity type this repository works with</typeparam>
public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : class
{
    private readonly DbContext _dbContext;
    private readonly ILogger<EntityRepository<TEntity>> _logger;
    private readonly bool _enableTracking;

    /// <summary>
    /// Creates a new instance of the repository
    /// </summary>
    /// <param name="dbContext">Database context to use for operations</param>
    /// <param name="logger">Logger for capturing operation information</param>
    /// <param name="enableTracking">Whether to enable entity tracking</param>
    public EntityRepository(
        DbContext dbContext,
        ILogger<EntityRepository<TEntity>> logger,
        bool enableTracking = false)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _enableTracking = enableTracking;

        ConfigureContextBehavior();
    }

    /// <summary>
    /// Base queryable for this entity type
    /// </summary>
    private IQueryable<TEntity> BaseQuery => GetBaseQuery();

    /// <inheritdoc />
    public async Task<TEntity> CreateAsync(TEntity entity, bool detach = true, bool autoSaveChanges = true)
    {
        try
        {
            _logger.LogDebug("Creating entity of type {EntityType}", typeof(TEntity).Name);

            var entry = await _dbContext.Set<TEntity>().AddAsync(entity);
            entry.State = EntityState.Added;

            if (autoSaveChanges)
                await _dbContext.SaveChangesAsync();

            if (detach)
                entry.State = EntityState.Detached;

            return entry.Entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating entity of type {EntityType}", typeof(TEntity).Name);
            throw new RepositoryOperationException($"Failed to create entity: {ex.Message}", ex);
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> CreateRangeAsync(IEnumerable<TEntity> entities, bool autoSaveChanges = true)
    {
        var entityList = entities.ToList();

        try
        {
            _logger.LogDebug("Creating {Count} entities of type {EntityType}", entityList.Count, typeof(TEntity).Name);

            await _dbContext.Set<TEntity>().AddRangeAsync(entityList);

            if (autoSaveChanges)
                await _dbContext.SaveChangesAsync();

            return entityList;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating multiple entities of type {EntityType}", typeof(TEntity).Name);
            throw new RepositoryOperationException($"Failed to create entities: {ex.Message}", ex);
        }
    }

    /// <inheritdoc />
    public async Task<TEntity> UpdateAsync(TEntity entity, bool detach = true, bool autoSaveChanges = true)
    {
        try
        {
            _logger.LogDebug("Updating entity of type {EntityType}", typeof(TEntity).Name);

            var entry = _dbContext.Set<TEntity>().Update(entity);
            entry.State = EntityState.Modified;

            if (autoSaveChanges)
                await _dbContext.SaveChangesAsync();

            if (detach)
                entry.State = EntityState.Detached;

            return entry.Entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating entity of type {EntityType}", typeof(TEntity).Name);
            throw new RepositoryOperationException($"Failed to update entity: {ex.Message}", ex);
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities, bool autoSaveChanges = true)
    {
        var entityList = entities.ToList();

        try
        {
            _logger.LogDebug("Updating {Count} entities of type {EntityType}", entityList.Count, typeof(TEntity).Name);

            _dbContext.Set<TEntity>().UpdateRange(entityList);

            if (autoSaveChanges)
                await _dbContext.SaveChangesAsync();

            return entityList;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating multiple entities of type {EntityType}", typeof(TEntity).Name);
            throw new RepositoryOperationException($"Failed to update entities: {ex.Message}", ex);
        }
    }

    /// <inheritdoc />
    public async Task<TEntity> DeleteAsync(TEntity entity, bool detach = true, bool autoSaveChanges = true)
    {
        try
        {
            _logger.LogDebug("Deleting entity of type {EntityType}", typeof(TEntity).Name);

            var entry = _dbContext.Set<TEntity>().Remove(entity);
            entry.State = EntityState.Deleted;

            if (autoSaveChanges)
                await _dbContext.SaveChangesAsync();

            if (detach)
                entry.State = EntityState.Detached;

            return entry.Entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting entity of type {EntityType}", typeof(TEntity).Name);
            throw new RepositoryOperationException($"Failed to delete entity: {ex.Message}", ex);
        }
    }

    /// <inheritdoc />
    public async Task<bool> DeleteByPredicateAsync(Expression<Func<TEntity, bool>> predicate, bool autoSaveChanges = true)
    {
        try
        {
            _logger.LogDebug("Deleting entities of type {EntityType} by predicate", typeof(TEntity).Name);

            var entities = await GetAsync(predicate);

            if (!entities.Any())
                return false;

            _dbContext.Set<TEntity>().RemoveRange(entities);

            if (autoSaveChanges)
                await _dbContext.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting entities of type {EntityType} by predicate", typeof(TEntity).Name);
            throw new RepositoryOperationException($"Failed to delete entities by predicate: {ex.Message}", ex);
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> DeleteRangeAsync(IEnumerable<TEntity> entities, bool autoSaveChanges = true)
    {
        var entityList = entities.ToList();

        try
        {
            _logger.LogDebug("Deleting {Count} entities of type {EntityType}", entityList.Count, typeof(TEntity).Name);

            _dbContext.Set<TEntity>().RemoveRange(entityList);

            if (autoSaveChanges)
                await _dbContext.SaveChangesAsync();

            return entityList;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting multiple entities of type {EntityType}", typeof(TEntity).Name);
            throw new RepositoryOperationException($"Failed to delete entities: {ex.Message}", ex);
        }
    }

    /// <inheritdoc />
    public Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        try
        {
            return ApplyPredicate(predicate).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving first entity of type {EntityType}", typeof(TEntity).Name);
            throw new RepositoryQueryException($"Failed to retrieve first entity: {ex.Message}", ex);
        }
    }

    /// <inheritdoc />
    public Task<TEntity> GetLastOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        try
        {
            return ApplyPredicate(predicate).LastOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving last entity of type {EntityType}", typeof(TEntity).Name);
            throw new RepositoryQueryException($"Failed to retrieve last entity: {ex.Message}", ex);
        }
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        try
        {
            return await ApplyPredicate(predicate).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving entities of type {EntityType}", typeof(TEntity).Name);
            throw new RepositoryQueryException($"Failed to retrieve entities: {ex.Message}", ex);
        }
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<TEntity>> GetSortedAsync<TKey>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy,
        SortDirection sortDirection = SortDirection.Ascending)
    {
        try
        {
            var query = ApplySorting(ApplyPredicate(predicate), orderBy, sortDirection);
            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving sorted entities of type {EntityType}", typeof(TEntity).Name);
            throw new RepositoryQueryException($"Failed to retrieve sorted entities: {ex.Message}", ex);
        }
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<TEntity>> GetWithIncludesAsync(
        IEnumerable<string> includes,
        Expression<Func<TEntity, bool>> predicate = null)
    {
        try
        {
            var query = ApplyIncludes(ApplyPredicate(predicate), includes);
            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving entities with includes of type {EntityType}", typeof(TEntity).Name);
            throw new RepositoryQueryException($"Failed to retrieve entities with includes: {ex.Message}", ex);
        }
    }

    /// <inheritdoc />
    public async Task<PagedResult<TEntity>> GetPagedAsync(
        int page,
        int pageSize,
        Expression<Func<TEntity, bool>> predicate = null)
    {
        try
        {
            return await CreatePagedResultAsync(page, pageSize, ApplyPredicate(predicate));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving paged entities of type {EntityType}", typeof(TEntity).Name);
            throw new RepositoryQueryException($"Failed to retrieve paged entities: {ex.Message}", ex);
        }
    }

    /// <inheritdoc />
    public async Task<PagedResult<TEntity>> GetPagedWithIncludesAsync(
        int page,
        int pageSize,
        IEnumerable<string> includes,
        Expression<Func<TEntity, bool>> predicate = null)
    {
        try
        {
            var query = ApplyIncludes(ApplyPredicate(predicate), includes);
            return await CreatePagedResultAsync(page, pageSize, query);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving paged entities with includes of type {EntityType}", typeof(TEntity).Name);
            throw new RepositoryQueryException($"Failed to retrieve paged entities with includes: {ex.Message}", ex);
        }
    }

    /// <inheritdoc />
    public async Task<PagedResult<TEntity>> GetPagedSortedAsync<TKey>(
        int page,
        int pageSize,
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy,
        SortDirection sortDirection = SortDirection.Ascending)
    {
        try
        {
            var query = ApplySorting(ApplyPredicate(predicate), orderBy, sortDirection);
            return await CreatePagedResultAsync(page, pageSize, query);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving paged sorted entities of type {EntityType}", typeof(TEntity).Name);
            throw new RepositoryQueryException($"Failed to retrieve paged sorted entities: {ex.Message}", ex);
        }
    }

    /// <inheritdoc />
    public Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        try
        {
            return predicate == null
                ? BaseQuery.LongCountAsync()
                : BaseQuery.Where(predicate).LongCountAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error counting entities of type {EntityType}", typeof(TEntity).Name);
            throw new RepositoryQueryException($"Failed to count entities: {ex.Message}", ex);
        }
    }

    private IQueryable<TEntity> GetBaseQuery()
    {
        return _enableTracking
            ? _dbContext.Set<TEntity>()
            : _dbContext.Set<TEntity>().AsNoTracking();
    }

    private void ConfigureContextBehavior()
    {
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = _enableTracking;
        _dbContext.ChangeTracker.LazyLoadingEnabled = _enableTracking;
        _dbContext.ChangeTracker.QueryTrackingBehavior = _enableTracking
            ? QueryTrackingBehavior.TrackAll
            : QueryTrackingBehavior.NoTracking;
    }

    private IQueryable<TEntity> ApplyPredicate(Expression<Func<TEntity, bool>> predicate)
    {
        return predicate != null ? BaseQuery.Where(predicate) : BaseQuery;
    }

    private IQueryable<TEntity> ApplySorting<TKey>(
        IQueryable<TEntity> query,
        Expression<Func<TEntity, TKey>> orderBy,
        SortDirection sortDirection)
    {
        return sortDirection == SortDirection.Ascending
            ? query.OrderBy(orderBy)
            : query.OrderByDescending(orderBy);
    }

    private IQueryable<TEntity> ApplyIncludes(
        IQueryable<TEntity> query,
        IEnumerable<string> includes)
    {
        return includes.Aggregate(query, (current, includePath) => current.Include(includePath));
    }

    private async Task<PagedResult<TEntity>> CreatePagedResultAsync(
        int page,
        int pageSize,
        IQueryable<TEntity> query)
    {
        // Input validation
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var result = new PagedResult<TEntity>
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = await query.CountAsync()
        };

        var skip = (page - 1) * pageSize;
        result.Items = await query.Skip(skip).Take(pageSize).ToListAsync();
        result.TotalPages = (int)Math.Ceiling((double)result.TotalCount / pageSize);
        result.HasPreviousPage = page > 1;
        result.HasNextPage = page < result.TotalPages;

        return result;
    }

    private (string Query, object[] ParameterValues) BuildStoredProcedureCommand(
        string procedureName,
        SqlParameter[] parameters)
    {
        var paramList = parameters.ToList();
        var paramPlaceholders = string.Join(", ", paramList.Select((p, i) => $"@p{i}"));
        var paramValues = paramList.Select(p => p.Value).ToArray();

        return ($"EXEC {procedureName} {paramPlaceholders}", paramValues);
    }
}