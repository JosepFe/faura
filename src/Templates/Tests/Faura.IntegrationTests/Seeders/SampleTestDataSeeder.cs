namespace Faura.IntegrationTest.Seeders;

using Faura.WebAPI.Domain;
using Faura.WebAPI.Domain.Entities;
using Faura.WebAPI.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Faura.Infrastructure.IntegrationTesting.Seeders;

/// <summary>
/// Seeds test data for Sample entity.
/// This seeder runs automatically during factory initialization.
/// </summary>
public class SampleTestDataSeeder : TestDataSeeder<SampleDbContext>
{
    protected override async Task SeedDataAsync(
        SampleDbContext context,
        IServiceProvider scopedProvider
    )
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(scopedProvider);

        var repo = scopedProvider.GetRequiredService<ISampleRepository>();

        // Seed multiple samples for testing
        await repo.CreateAsync(new Sample("Sample One", "First sample item", "CategoryA"));
        await repo.CreateAsync(new Sample("Sample Two", "Second sample item", "CategoryB"));
        await repo.CreateAsync(new Sample("Sample Three", "Third sample item", "CategoryA"));
    }
}
