namespace Faura.IntegrationTest.Configuration;

using DotNet.Testcontainers.Containers;
using Faura.Infrastructure.IntegrationTesting.Factory;
using Faura.Infrastructure.IntegrationTesting.Options;
using Faura.Infrastructure.IntegrationTesting.Seeders;
using Faura.Infrastructure.IntegrationTesting.TestContainers.Configurations;
using Faura.Infrastructure.IntegrationTesting.TestContainers.Core;
using Faura.Infrastructure.UnitOfWork.Common;
using Faura.Infrastructure.UnitOfWork.Enums;
using Faura.IntegrationTest.Seeders;
using Faura.WebAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public class CustomWebApplicationFactory<TEntryPoint> : BaseWebApplicationFactory<TEntryPoint>
    where TEntryPoint : class
{
    private IContainer? _postgresContainer;

    public override async Task DisposeAsync()
    {
        if (_postgresContainer is not null)
        {
            await _postgresContainer.StopAsync();
        }

        await base.DisposeAsync();
    }

    protected override async Task<IConfiguration> ConfigureTestContainersAsync(
        IConfiguration configuration)
    {
        var containerOptions = configuration.GetSection("Containers").Get<TestContainerOptions>();
        var pgOptions = containerOptions!.Postgres;

        var pgConfig = new PostgresContainerConfiguration(pgOptions);
        var containerInstance = new TestContainerInstance<PostgresContainerConfiguration>(pgConfig);

        _postgresContainer = containerInstance.Container;

        await StartContainerWithRetriesAsync(containerInstance);

        return new ConfigurationBuilder()
            .AddConfiguration(configuration)
            .AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["ConnectionStrings:Employee"] = containerInstance.ConnectionString,
                })
            .Build();
    }

    protected override void ConfigureTestServices(
        IServiceCollection services,
        IConfiguration configuration)
        => services.AddScoped<ITestDataSeeder, EmployeeTestDataSeeder>();

    protected override void ConfigureTestDatabase(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.RemoveAll(typeof(DbContextOptions<EmployeeDbContext>));
        services.ConfigureDatabase<EmployeeDbContext>(
            configuration.GetConnectionString("Employee") !,
            DatabaseType.PostgreSQL,
            ServiceLifetime.Scoped);
    }

    private static async Task StartContainerWithRetriesAsync<TConfig>(
        TestContainerInstance<TConfig> containerInstance)
        where TConfig : ITestContainerConfiguration
    {
        const int maxRetries = 5;
        var delay = TimeSpan.FromSeconds(5);
        Exception? lastException = null;

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                Console.WriteLine($"Starting container (attempt {attempt}/{maxRetries})...");
                await containerInstance.StartAsync();
                Console.WriteLine($"Container started successfully.");
                return;
            }
            catch (Exception ex)
            {
                lastException = ex;
                Console.WriteLine($"Failed to start container: {ex.Message}");
                if (attempt < maxRetries)
                    await Task.Delay(delay);
            }
        }

        throw new InvalidOperationException($"Could not start container after {maxRetries} attempts.", lastException);
    }
}
