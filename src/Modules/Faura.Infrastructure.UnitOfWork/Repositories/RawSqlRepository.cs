namespace Faura.Infrastructure.UnitOfWork.Repositories;

using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;

public class RawSqlRepository<TModel> : IRawSqlRepository<TModel> where TModel : class
{
    private readonly DbContext _context;

    public RawSqlRepository(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<TModel>> QueryAsync(string sql, object? parameters = null, IDbTransaction? transaction = null)
    {
        var connection = _context.Database.GetDbConnection();
        return await connection.QueryAsync<TModel>(sql, parameters, transaction);
    }

    public async Task<TModel?> QueryFirstOrDefaultAsync(string sql, object? parameters = null, IDbTransaction? transaction = null)
    {
        var connection = _context.Database.GetDbConnection();
        return await connection.QueryFirstOrDefaultAsync<TModel>(sql, parameters, transaction);
    }

    public async Task<int> ExecuteAsync(string sql, object? parameters = null, IDbTransaction? transaction = null)
    {
        var connection = _context.Database.GetDbConnection();
        return await connection.ExecuteAsync(sql, parameters, transaction);
    }
}