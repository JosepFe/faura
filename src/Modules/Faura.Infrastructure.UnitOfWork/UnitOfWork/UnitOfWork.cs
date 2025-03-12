namespace Faura.Infrastructure.UnitOfWork.UnitOfWork;

using System.Data;
using Faura.Infrastructure.UnitOfWork.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private const int DefaultSecondsTimeout = 30;

    protected UnitOfWork(TContext context)
    {
        Context = context ?? throw new NullContextException(nameof(context));
    }

    private TContext Context { get; }

    public async Task<IDbTransaction> GetDbTransaction(int secondsTimeout = DefaultSecondsTimeout)
    {
        Context.Database.SetCommandTimeout(secondsTimeout);

        var dbContextTransaction = await Context.Database.BeginTransactionAsync();
        return dbContextTransaction.GetDbTransaction();
    }

    public Task SaveChanges()
    {
        //Auditable entities
        return Context.SaveChangesAsync();
    }

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return Context.Database.CreateExecutionStrategy();
    }

    public async Task CommitTransaction(IDbTransaction transaction)
    {
        if (transaction == null)
            throw new NullTransactionException("Transaction cannot be null to perform transaction operations.");

        try
        {
            await SaveChanges();

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            Context.Database.SetCommandTimeout(DefaultSecondsTimeout);
            transaction.Dispose();
        }
    }
}