namespace Faura.Infrastructure.UnitOfWork.Projectors;

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

public class Projector<TEntity> : IProjector<TEntity> where TEntity : class
{
    private readonly DbContext _context;

    public Projector(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<TResult>> GetProjectionAsync<TResult>(
        Func<IQueryable<TEntity>, IQueryable<TResult>> projection,
        int? page = null,
        int? pageSize = null)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        // Apply projection first
        var projectionQuery = projection(query);

        // Then apply pagination if requested
        if (page.HasValue && pageSize.HasValue)
        {
            projectionQuery = projectionQuery
                .Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
        }

        return await projectionQuery.ToListAsync();
    }
}