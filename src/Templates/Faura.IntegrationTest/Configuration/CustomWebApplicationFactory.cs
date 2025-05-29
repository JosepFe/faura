using DotNet.Testcontainers.Containers;
using Faura.Infrastructure.IntegrationTesting.Factory;
using Faura.Infrastructure.IntegrationTesting.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YourNamespace.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Faura.Infrastructure.UnitOfWork.Enums;
using Faura.Infrastructure.UnitOfWork.Common;
using Faura.Infrastructure.IntegrationTesting.Seeders;
using Faura.IntegrationTest.Seeders;
using Faura.Infrastructure.IntegrationTesting.TestContainers.Configurations;
using Faura.Infrastructure.IntegrationTesting.TestContainers.Core;

namespace Faura.IntegrationTest.Configuration;
public class CustomWebApplicationFactory<TEntryPoint> : BaseWebApplicationFactory<TEntryPoint> where TEntryPoint : class
{
    private IContainer? _postgresContainer;

    public override async Task DisposeAsync()
    {
        if (_postgresContainer is not null)
        {
            await _postgresContainer.StopAsync();
        }
    }

    protected override async Task<IConfiguration> ConfigureTestContainersAsync(IConfiguration configuration)
    {
        var containerOptions = configuration.GetSection("Containers").Get<TestContainerOptions>();
        var pgOptions = containerOptions!.Postgres;

        var pgConfig = new PostgresContainerConfiguration(pgOptions);
        var containerManager = new TestContainerInstance<PostgresContainerConfiguration>(pgConfig);

        await containerManager.StartAsync();

        return new ConfigurationBuilder()
            .AddConfiguration(configuration)
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Employee"] = containerManager.ConnectionString
            })
            .Build();
    }

    protected override void ConfigureTestServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITestDataSeeder, EmployeeTestDataSeeder>();
    }

    protected override void ConfigureTestDatabase(IServiceCollection services, IConfiguration configuration)
    {
        services.RemoveAll(typeof(DbContextOptions<EmployeeDbContext>));
        services.ConfigureDatabase<EmployeeDbContext>(configuration.GetConnectionString("Employee")!, DatabaseType.PostgreSQL, ServiceLifetime.Scoped);
    }
}
