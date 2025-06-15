namespace Faura.WebAPI.Bootstrappers;

using Faura.Infrastructure.UnitOfWork;
using Faura.Infrastructure.UnitOfWork.Common;
using Faura.Infrastructure.UnitOfWork.Enums;
using Faura.WebAPI.Domain.Repositories;
using Faura.WebAPI.Infrastructure.Persistence;
using Faura.WebAPI.Infrastructure.Repositories;

public static class ApplicationBootstrapper
{
    public static WebApplicationBuilder RegisterApplicationDependencies(
        this WebApplicationBuilder builder)
    {
        builder.Services.SetupDatabase(builder.Configuration);

        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IEmployeeUoW, EmployeeUoW>();
        return builder;
    }

    /// <summary>
    /// Setup here your database connections.
    /// </summary>
    private static void SetupDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.SetupUnitOfWork();
        services
            .ConfigureDatabase<EmployeeDbContext>(
                configuration,
                "Employee",
                DatabaseType.PostgreSQL,
                ServiceLifetime.Scoped)
            .ConfigureAwait(false);
    }
}
