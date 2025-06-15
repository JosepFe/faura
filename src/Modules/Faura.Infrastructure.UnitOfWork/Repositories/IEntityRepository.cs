namespace Faura.Infrastructure.UnitOfWork.Repositories;

using Faura.Infrastructure.UnitOfWork.Enums;
using Faura.Infrastructure.UnitOfWork.Models;
using System.Linq.Expressions;

/// <summary>
/// Repository interface for entity operations.
/// </summary>
/// <typeparam name="TEntity">Entity type.</typeparam>
public interface IEntityRepository<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Creates a new entity.
    /// </summary>
    /// <param name="entity">Entity to create.</param>
    /// <returns>Created entity.</returns>
    Task<TEntity> CreateAsync(TEntity entity, bool detach = true, bool autoSaveChanges = true);

    /// <summary>
    /// Creates multiple entities.
    /// </summary>
    /// <param name="entities">Entities to create.</param>
    /// <returns>Created entities.</returns>
    Task<IEnumerable<TEntity>> CreateRangeAsync(IEnumerable<TEntity> entities, bool autoSaveChanges = true);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">Entity to update.</param>
    /// <returns>Updated entity.</returns>
    Task<TEntity> UpdateAsync(TEntity entity, bool detach = true, bool autoSaveChanges = true);

    /// <summary>
    /// Updates multiple entities.
    /// </summary>
    /// <param name="entities">Entities to update.</param>
    /// <returns>Updated entities.</returns>
    Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities, bool autoSaveChanges = true);

    /// <summary>
    /// Deletes an entity.
    /// </summary>
    /// <param name="entity">Entity to delete.</param>
    /// <returns>Deleted entity.</returns>
    Task<TEntity> DeleteAsync(TEntity entity, bool detach = true, bool autoSaveChanges = true);

    /// <summary>
    /// Deletes entities matching a predicate.
    /// </summary>
    /// <param name="predicate">Filter predicate.</param>
    /// <returns>True if entities were deleted.</returns>
    Task<bool> DeleteByPredicateAsync(Expression<Func<TEntity, bool>> predicate, bool autoSaveChanges = true);

    /// <summary>
    /// Deletes multiple entities.
    /// </summary>
    /// <param name="entities">Entities to delete.</param>
    /// <returns>Deleted entities.</returns>
    Task<IEnumerable<TEntity>> DeleteRangeAsync(IEnumerable<TEntity> entities, bool autoSaveChanges = true);

    /// <summary>
    /// Gets the first entity matching the predicate or default if none.
    /// </summary>
    /// <param name="predicate">Filter predicate.</param>
    /// <returns>First matching entity or default.</returns>
    Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null);

    /// <summary>
    /// Gets the last entity matching the predicate or default if none.
    /// </summary>
    /// <param name="predicate">Filter predicate.</param>
    /// <returns>Last matching entity or default.</returns>
    Task<TEntity?> GetLastOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null);

    /// <summary>
    /// Gets all entities matching the predicate.
    /// </summary>
    /// <param name="predicate">Filter predicate.</param>
    /// <returns>List of matching entities.</returns>
    Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null);

    /// <summary>
    /// Gets sorted entities matching the predicate.
    /// </summary>
    /// <typeparam name="TKey">Type of the sort key.</typeparam>
    /// <param name="predicate">Filter predicat.</param>
    /// <param name="orderBy">Property to sort by.</param>
    /// <param name="sortDirection">Sort direction.</param>
    /// <returns>Sorted list of matching entities.</returns>
    Task<IReadOnlyList<TEntity>> GetSortedAsync<TKey>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy,
        SortDirection sortDirection = SortDirection.Ascending);

    /// <summary>
    /// Gets entities with included related entities.
    /// </summary>
    /// <param name="includes">Properties to include.</param>
    /// <param name="predicate">Filter predicate.</param>
    /// <returns>List of entities with included properties.</returns>
    Task<IReadOnlyList<TEntity>> GetWithIncludesAsync(
        IEnumerable<string> includes,
        Expression<Func<TEntity, bool>>? predicate = null);

    /// <summary>
    /// Gets a page of entities matching the predicate.
    /// </summary>
    /// <param name="page">Page number (1-based).</param>
    /// <param name="pageSize">Page size.</param>
    /// <param name="predicate">Filter predicate.</param>
    /// <returns>Paged result.</returns>
    Task<PagedResult<TEntity>> GetPagedAsync(
        int page,
        int pageSize,
        Expression<Func<TEntity, bool>>? predicate = null);

    /// <summary>
    /// Gets a page of entities with included related entities.
    /// </summary>
    /// <param name="page">Page number (1-based).</param>
    /// <param name="pageSize">Page size.</param>
    /// <param name="includes">Properties to include.</param>
    /// <param name="predicate">Filter predicate.</param>
    /// <returns>Paged result with included properties.</returns>
    Task<PagedResult<TEntity>> GetPagedWithIncludesAsync(
        int page,
        int pageSize,
        IEnumerable<string> includes,
        Expression<Func<TEntity, bool>>? predicate = null);

    /// <summary>
    /// Gets a sorted page of entities matching the predicate.
    /// </summary>
    /// <typeparam name="TKey">Type of the sort key.</typeparam>
    /// <param name="page">Page number (1-based).</param>
    /// <param name="pageSize">Page size.</param>
    /// <param name="predicate">Filter predicate.</param>
    /// <param name="orderBy">Property to sort by.</param>
    /// <param name="sortDirection">Sort direction.</param>
    /// <returns>Sorted paged result.</returns>
    Task<PagedResult<TEntity>> GetPagedSortedAsync<TKey>(
        int page,
        int pageSize,
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> orderBy,
        SortDirection sortDirection = SortDirection.Ascending);

    /// <summary>
    /// Counts entities matching the predicate.
    /// </summary>
    /// <param name="predicate">Filter predicate.</param>
    /// <returns>Number of matching entities.</returns>
    Task<long> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);
}
