using Faura.Infrastructure.IntegrationTesting.Constants;
using Faura.Infrastructure.IntegrationTesting.Seeders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Faura.Infrastructure.IntegrationTesting.Factory;

/// <summary>
/// Base factory for integration tests with container setup and service customization.
/// </summary>
public abstract class BaseWebApplicationFactory<TEntryPoint>
    : WebApplicationFactory<TEntryPoint>,
        IAsyncLifetime
    where TEntryPoint : class
{
    /// <summary>
    /// Final configuration used during the test run.
    /// </summary>
    protected IConfiguration? Configuration;

    public async Task InitializeAsync()
    {
        Environment.SetEnvironmentVariable(
            "ASPNETCORE_ENVIRONMENT",
            IntegrationTestConstants.Environment
        );

        var testProjectPath = Directory.GetCurrentDirectory();
        var settingsFile = Path.Combine(
            testProjectPath,
            $"appsettings.{IntegrationTestConstants.Environment}.json"
        );

        var configBuilder = new ConfigurationBuilder()
            .AddJsonFile(settingsFile, optional: false)
            .AddEnvironmentVariables();

        await ConfigureConfigurationAsync(configBuilder);

        var baseConfig = configBuilder.Build();
        Configuration = await ConfigureTestContainersAsync(baseConfig);
    }

    public virtual Task DisposeAsync() => Task.CompletedTask;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(IntegrationTestConstants.Environment);

        builder.ConfigureAppConfiguration(
            (_, config) =>
            {
                config.Sources.Clear();
                config.AddConfiguration(Configuration!);
            }
        );

        builder.ConfigureServices(services =>
        {
            ConfigureTestDatabase(services, Configuration!);
            ConfigureTestServices(services, Configuration!);

            using var provider = services.BuildServiceProvider(validateScopes: true);
            using var scope = provider.CreateScope();
            RunSeedersAsync(scope.ServiceProvider).GetAwaiter().GetResult();
        });
    }

    /// <summary>
    /// Allows custom additions to the configuration before it's built.
    /// </summary>
    protected virtual Task ConfigureConfigurationAsync(IConfigurationBuilder builder) =>
        Task.CompletedTask;

    /// <summary>
    /// Starts containers and returns updated configuration (e.g., with connection strings).
    /// </summary>
    protected virtual Task<IConfiguration> ConfigureTestContainersAsync(
        IConfiguration configuration
    ) => Task.FromResult(configuration);

    /// <summary>
    /// Registers test-specific or mocked services.
    /// </summary>
    protected virtual void ConfigureTestServices(
        IServiceCollection services,
        IConfiguration configuration
    ) { }

    /// <summary>
    /// Registers database context and ensures schema is created.
    /// </summary>
    protected virtual void ConfigureTestDatabase(
        IServiceCollection services,
        IConfiguration configuration
    ) { }

    private async Task RunSeedersAsync(IServiceProvider serviceProvider)
    {
        foreach (var seeder in serviceProvider.GetServices<ITestDataSeeder>())
        {
            try
            {
                await seeder.SeedAsync(serviceProvider);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeder {seeder.GetType().Name} failed: {ex.Message}");
                throw;
            }
        }
    }
}
