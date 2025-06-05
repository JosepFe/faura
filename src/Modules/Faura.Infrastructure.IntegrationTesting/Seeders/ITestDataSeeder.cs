namespace Faura.Infrastructure.IntegrationTesting.Seeders;

public interface ITestDataSeeder
{
    Task SeedAsync(IServiceProvider serviceProvider);
}
