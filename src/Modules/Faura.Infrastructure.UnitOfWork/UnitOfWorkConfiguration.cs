namespace Faura.Infrastructure.UnitOfWork;

using Faura.Infrastructure.UnitOfWork.Projectors;
using Faura.Infrastructure.UnitOfWork.Repositories;
using Microsoft.Extensions.DependencyInjection;

public static class UnitOfWorkConfiguration
{
    public static void SetupUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped(typeof(IEntityRepository<>), typeof(EntityRepository<>));
        services.AddScoped<IRawSqlRepository, RawSqlRepository>();
        services.AddScoped<IProjector, Projector>();
    }
}