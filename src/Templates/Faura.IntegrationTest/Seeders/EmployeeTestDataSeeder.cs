using Faura.Infrastructure.IntegrationTesting.Seeders;
using Faura.WebAPI.Domain;
using Faura.WebAPI.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using YourNamespace.Data;

namespace Faura.IntegrationTest.Seeders;

public class EmployeeTestDataSeeder : TestDataSeeder<EmployeeDbContext>
{
    protected override Task SeedDataAsync(EmployeeDbContext context, IServiceProvider scopedProvider)
    {
        var repo = scopedProvider.GetRequiredService<IEmployeeRepository>();

        return repo.CreateAsync(new Employee("User", "Example", "user@example.com"));
    }
}
