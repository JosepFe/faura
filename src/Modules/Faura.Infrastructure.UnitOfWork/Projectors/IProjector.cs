namespace Faura.Infrastructure.UnitOfWork.Projectors;

public interface IProjector<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Gets a projection from the entity.
    /// </summary>
    /// <typeparam name="TResult">The result type of the projection.</typeparam>
    /// <param name="projection">The projection function.</param>
    /// <param name="page">Optional page number for pagination.</param>
    /// <param name="pageSize">Optional page size for pagination.</param>
    /// <returns>A task that represents the asynchronous operation, containing the projected results.</returns>
    Task<IEnumerable<TResult>> GetProjectionAsync<TResult>(
        Func<IQueryable<TEntity>, IQueryable<TResult>> projection,
        int? page = null,
        int? pageSize = null);
}
