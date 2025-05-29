namespace Faura.WebAPI.Boostrappers;

using Faura.Infrastructure.UnitOfWork;
using Faura.Infrastructure.UnitOfWork.Common;
using Faura.Infrastructure.UnitOfWork.Enums;
using Faura.WebAPI.Domain;
using Faura.WebAPI.Infrastructure.Persistence;
using YourNamespace.Data;

public static class ApplicationBoostrapper
{
    public static WebApplicationBuilder RegisterApplicationDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.SetupDatabase(builder.Configuration);

        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IEmployeeUoW, EmployeeUoW>();
        return builder;
    }

    /// <summary>
    /// Setup here your database connections.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    private static void SetupDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.SetupUnitOfWork();
        services.ConfigureDatabase<EmployeeDbContext>(configuration, "Employee", DatabaseType.PostgreSQL, ServiceLifetime.Scoped).ConfigureAwait(false);
    }
}