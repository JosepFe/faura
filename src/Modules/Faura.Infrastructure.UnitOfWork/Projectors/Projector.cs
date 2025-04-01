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

        if (page.HasValue && pageSize.HasValue)
        {
            query = query.Skip((page.Value - 1) * pageSize.Value)
                        .Take(pageSize.Value);
        }

        var projectionQuery = projection(query);
        return await projectionQuery.ToListAsync();
    }
}