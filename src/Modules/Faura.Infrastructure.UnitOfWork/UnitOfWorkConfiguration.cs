namespace Faura.Infrastructure.UnitOfWork;

using Faura.Infrastructure.UnitOfWork.Generated;
using Faura.Infrastructure.UnitOfWork.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

public static class UnitOfWorkConfiguration
{
    public static void SetupUnitOfWork(this IServiceCollection services)
    {
        services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
    }
}