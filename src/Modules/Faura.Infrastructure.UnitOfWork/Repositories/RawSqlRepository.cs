namespace Faura.Infrastructure.UnitOfWork.Repositories;

using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;

public class RawSqlRepository<TModel> : IRawSqlRepository<TModel>
    where TModel : class
{
    private readonly DbContext _context;

    public RawSqlRepository(DbContext context)
        => _context = context ?? throw new ArgumentNullException(nameof(context));

    public Task<IEnumerable<TModel>> QueryAsync(string sql, object? parameters = null, IDbTransaction? transaction = null)
    {
        var connection = _context.Database.GetDbConnection();
        return connection.QueryAsync<TModel>(sql, parameters, transaction);
    }

    public Task<TModel?> QueryFirstOrDefaultAsync(string sql, object? parameters = null, IDbTransaction? transaction = null)
    {
        var connection = _context.Database.GetDbConnection();
        return connection.QueryFirstOrDefaultAsync<TModel>(sql, parameters, transaction);
    }

    public Task<int> ExecuteAsync(string sql, object? parameters = null, IDbTransaction? transaction = null)
    {
        var connection = _context.Database.GetDbConnection();
        return connection.ExecuteAsync(sql, parameters, transaction);
    }
}
