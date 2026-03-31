namespace Faura.Infrastructure.UnitOfWork.Configuration;

using Faura.Infrastructure.UnitOfWork.Core;
using Faura.Infrastructure.UnitOfWork.Projectors;
using Faura.Infrastructure.UnitOfWork.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class UnitOfWorkConfiguration
{
    /// <summary>
    /// Registers the UnitOfWork pattern implementation for the specified DbContext
    /// </summary>
    /// <typeparam name="TContext">The DbContext type</typeparam>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services) 
        where TContext : DbContext
    {
        services.AddScoped<IUnitOfWork, Core.UnitOfWork<TContext>>();
        return services;
    }

    /// <summary>
    /// Registers repository implementations (EntityRepository, RawSqlRepository, Projector)
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IEntityRepository<>), typeof(EntityRepository<>));
        services.AddScoped(typeof(IRawSqlRepository<>), typeof(RawSqlRepository<>));
        services.AddScoped(typeof(IProjector<>), typeof(Projector<>));
        return services;
    }

    /// <summary>
    /// Registers both UnitOfWork and Repositories in one call
    /// </summary>
    /// <typeparam name="TContext">The DbContext type</typeparam>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddUnitOfWorkWithRepositories<TContext>(this IServiceCollection services) 
        where TContext : DbContext
    {
        services.AddUnitOfWork<TContext>();
        services.AddRepositories();
        return services;
    }

    /// <summary>
    /// [Obsolete] Use AddUnitOfWorkWithRepositories instead
    /// </summary>
    [Obsolete("Use AddUnitOfWorkWithRepositories<TContext>() instead. This method will be removed in a future version.")]
    public static void SetupUnitOfWork(this IServiceCollection services)
    {
        services.AddRepositories();
    }
}