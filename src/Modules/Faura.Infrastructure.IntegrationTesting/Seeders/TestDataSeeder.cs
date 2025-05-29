using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Faura.Infrastructure.IntegrationTesting.Seeders;

public abstract class TestDataSeeder<TContext> : ITestDataSeeder where TContext : DbContext
{
    public async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();

        await context.Database.EnsureCreatedAsync();

        await SeedDataAsync(context, scope.ServiceProvider);
    }

    /// <summary>
    /// Override this method to seed data using the provided DbContext.
    /// </summary>
    protected abstract Task SeedDataAsync(TContext context, IServiceProvider scopedProvider);
}
