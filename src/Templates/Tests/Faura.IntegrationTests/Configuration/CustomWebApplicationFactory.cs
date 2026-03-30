namespace Faura.IntegrationTest.Configuration;

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

/// <summary>
/// Custom WebApplicationFactory for integration tests.
/// Configures test containers (PostgreSQL) and overrides services for testing.
/// </summary>
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
                    ["ConnectionStrings:Sample"] = containerInstance.ConnectionString,
                }
            )
            .Build();
    }

    protected override void ConfigureTestServices(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<ITestDataSeeder, SampleTestDataSeeder>();
    }

    protected override void ConfigureTestDatabase(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.RemoveAll(typeof(DbContextOptions<SampleDbContext>));
        services.ConfigureDatabase<SampleDbContext>(
            configuration.GetConnectionString("Sample")!,
            DatabaseType.PostgreSQL,
            ServiceLifetime.Scoped
        );
    }
}
