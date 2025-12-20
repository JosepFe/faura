namespace Faura.WebAPI.Bootstrappers;

using Faura.Infrastructure.UnitOfWork.Common;
using Faura.Infrastructure.UnitOfWork.Enums;
using Faura.WebAPI.Domain;
using Faura.WebAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public static class InfrastructureBootstrapper
{
    public static IServiceCollection RegisterInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddRepositories();

        return services;
    }

    public static async Task<WebApplication> ConfigureInfrastructureAsync(this WebApplication app, IWebHostEnvironment env)
    {
        await MigrateDatabaseAsync(app, env);
        return app;
    }

    private static async Task MigrateDatabaseAsync(WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();
            await dbContext.Database.MigrateAsync();
        }
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDatabase<EmployeeDbContext>(configuration, "Employee", DatabaseType.PostgreSQL);
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        return services;
    }
}