namespace Template.Shared.Infrastructure.Repositories;

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Template.Shared.Domain.Entities;
using Template.Shared.Domain.Repositories;

/// <summary>
/// Generic repository implementation for entity operations.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <param name="context">The database context.</param>
public class EntityRepository<TEntity>(DbContext context) : IEntityRepository<TEntity>
    where TEntity : EntityBase
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    /// <inheritdoc/>
    public async Task<TEntity?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    /// <inheritdoc/>
    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}
