namespace Faura.IntegrationTest.Seeders;

using Faura.Infrastructure.IntegrationTesting.Seeders;
using Faura.WebAPI.Domain.Entities;
using Faura.WebAPI.Domain.Repositories;
using Faura.WebAPI.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

public class EmployeeTestDataSeeder : TestDataSeeder<EmployeeDbContext>
{
    protected override Task SeedDataAsync(
        EmployeeDbContext context,
        IServiceProvider scopedProvider)
    {
        var repo = scopedProvider.GetRequiredService<IEmployeeRepository>();

        return repo.CreateAsync(new Employee("User", "Example", "user@example.com"));
    }
}
