using Faura.Infrastructure.UnitOfWork.Common;
using Faura.Infrastructure.UnitOfWork.Enums;
using Faura.IntegrationTest.Seeders;
using Faura.WebAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Faura.Infrastructure.IntegrationTesting.Factory;
using Faura.Infrastructure.IntegrationTesting.Options;
using Faura.Infrastructure.IntegrationTesting.Seeders;
using Faura.Infrastructure.IntegrationTesting.TestContainers.Configurations;
using Faura.Infrastructure.IntegrationTesting.TestContainers.Core;

namespace Faura.IntegrationTest.Configuration;

public class CustomWebApplicationFactory : BaseWebApplicationFactory<Program>
{
    protected override async Task<IConfiguration> ConfigureTestContainersAsync(
        IConfiguration configuration
    )
    {
        var containerOptions = configuration.GetSection("Containers").Get<TestContainerOptions>();
        var pgOptions = containerOptions!.Postgres;

        var pgConfig = new PostgresContainerConfiguration(pgOptions);
        var containerInstance = new TestContainerInstance<PostgresContainerConfiguration>(pgConfig);

        await containerInstance.StartAsync();

        return new ConfigurationBuilder()
            .AddConfiguration(configuration)
            .AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["ConnectionStrings:Employee"] = containerInstance.ConnectionString,
                }
            )
            .Build();
    }

    protected override void ConfigureTestServices(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<ITestDataSeeder, EmployeeTestDataSeeder>();
    }

    protected override void ConfigureTestDatabase(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.RemoveAll(typeof(DbContextOptions<EmployeeDbContext>));
        services.ConfigureDatabase<EmployeeDbContext>(
            configuration.GetConnectionString("Employee")!,
            DatabaseType.PostgreSQL,
            ServiceLifetime.Scoped
        );
    }
}
