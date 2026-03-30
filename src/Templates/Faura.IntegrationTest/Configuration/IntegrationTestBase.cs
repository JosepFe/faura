namespace Faura.IntegrationTest.Configuration;

using Faura.WebAPI.Domain;
using Faura.WebAPI.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Base class for integration tests.
/// Provides access to HttpClient, DbContext, and Repository instances.
/// Implements IAsyncLifetime for proper test setup and teardown.
/// </summary>
public abstract class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    protected HttpClient Client { get; }
    protected IServiceScope Scope { get; }
    protected SampleDbContext DbContext { get; }
    protected ISampleRepository SampleRepository { get; }

    protected IntegrationTestBase(CustomWebApplicationFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        
        Client = factory.CreateClient();
        Scope = factory.Services.CreateScope();
        DbContext = Scope.ServiceProvider.GetRequiredService<SampleDbContext>();
        SampleRepository = Scope.ServiceProvider.GetRequiredService<ISampleRepository>();
    }

    /// <summary>
    /// Called before each test. Initializes database and seeds test data.
    /// </summary>
    public async Task InitializeAsync()
    {
        await DbContext.Database.EnsureCreatedAsync();
        await SeedTestDataAsync();
    }

    /// <summary>
    /// Override this method to seed custom test data for specific test classes.
    /// </summary>
    protected virtual Task SeedTestDataAsync() => Task.CompletedTask;

    /// <summary>
    /// Called after each test. Disposes resources.
    /// </summary>
    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
        Scope?.Dispose();
        Client?.Dispose();
    }
}