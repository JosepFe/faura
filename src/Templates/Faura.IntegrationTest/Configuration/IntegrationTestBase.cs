namespace Faura.IntegrationTest.Configuration;

using Faura.WebAPI.Domain;
using Faura.WebAPI.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

public abstract class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    protected readonly HttpClient Client;
    private readonly IServiceScope _scope;
    protected readonly EmployeeDbContext DbContext;
    protected readonly IEmployeeRepository EmployeeRepository;

    protected IntegrationTestBase(CustomWebApplicationFactory factory)
    {
        Client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        DbContext = _scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();
        EmployeeRepository = _scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
    }

    public async Task InitializeAsync()
    {
        await DbContext.Database.EnsureCreatedAsync();
        await SeedTestDataAsync();
    }

    protected virtual async Task SeedTestDataAsync()
    {
        await Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _scope?.Dispose();
        return Task.CompletedTask;
    }
}