namespace Faura.Infrastructure.UnitOfWork.Repositories;

using System.Data;

public interface IRawSqlRepository<TModel>
    where TModel : class
{
    /// <summary>
    /// This method executes a SQL query against the database using the Dapper library.
    /// </summary>
    /// <returns>
    /// It returns an IEnumerable of the provided object containing the results of the query.
    /// </returns>
    Task<IEnumerable<TModel>> QueryAsync(string sql, object? parameters = null, IDbTransaction? transaction = null);

    /// <summary>
    /// This method executes a SQL query against the database using the Dapper library.
    /// </summary>
    /// <returns>
    /// Returns the first result, or null if the query returns no results.
    /// </returns>
    Task<TModel?> QueryFirstOrDefaultAsync(string sql, object? parameters = null, IDbTransaction? transaction = null);

    /// <summary>
    /// This method executes a SQL query against the database using the Dapper library.
    /// </summary>
    /// <returns>
    /// It returns the number of affected rows.
    /// </returns>
    Task<int> ExecuteAsync(string sql, object? parameters = null, IDbTransaction? transaction = null);
}
