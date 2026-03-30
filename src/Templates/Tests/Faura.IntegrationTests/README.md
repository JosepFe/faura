# 🧪 Faura.IntegrationTests Template

Base template for integration tests with Testcontainers, xUnit, and Faura.Infrastructure.IntegrationTesting. **Not a standalone application** - integrate it into your project solution.

> ⚠️ This template has no `.sln` file of its own. It's added to the solution of the application you want to test.

## ✨ Includes

- ✅ CustomWebApplicationFactory with Testcontainers
- ✅ IntegrationTestBase with C# 12 (primary constructors)
- ✅ Complete test examples (CRUD + transactions)
- ✅ Seeders for test data
- ✅ Multi-container support (PostgreSQL, MongoDB, Redis, SQL Server)
- ✅ FauraResult pattern integration

## 🚀 How to Use

### 1. Copy the template to your project
```bash
cp -r src/Templates/Tests/Faura.IntegrationTests ./MyProject.IntegrationTests
```

### 2. Add it to your solution
```bash
dotnet sln add MyProject.IntegrationTests/MyProject.IntegrationTests.csproj
```

### 3. Update references in .csproj
```xml
<ItemGroup>
  <ProjectReference Include="..\MyProject.API\MyProject.API.csproj" />
  <ProjectReference Include="..\..\Modules\Faura.Infrastructure.IntegrationTesting\..." />
</ItemGroup>
```

### 4. Configure CustomWebApplicationFactory
Edit `Configuration/CustomWebApplicationFactory.cs` to use your DbContext:

```csharp
public class CustomWebApplicationFactory : BaseWebApplicationFactory<Program>
{
    protected override async Task<IConfiguration> ConfigureTestContainersAsync(IConfiguration configuration)
    {
        var pgConfig = new PostgresContainerConfiguration(containerOptions.Postgres);
        var container = new TestContainerInstance<PostgresContainerConfiguration>(pgConfig);
        await container.StartAsync();

        return new ConfigurationBuilder()
            .AddConfiguration(configuration)
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:MyDb"] = container.ConnectionString,
            })
            .Build();
    }

    protected override void ConfigureTestServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITestDataSeeder, MyTestDataSeeder>();
    }
}
```

### 5. Create your tests
```csharp
public class MyEntityTests(CustomWebApplicationFactory factory) 
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task CreateEntity_ShouldReturnOk()
    {
        // Arrange
        var request = new { Name = "Test" };
        
        // Act
        var response = await Client.PostAsJsonAsync("/api/myentity", request);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
```

### 6. Run the tests
```bash
dotnet test
```

## 📂 Structure

```
Configuration/          # CustomWebApplicationFactory + IntegrationTestBase
Seeders/               # Test data (ITestDataSeeder)
UseCases/              # Tests organized by use cases
appsettings.Test.json  # Container configuration
```

## 🐳 Testcontainers

Tests use ephemeral Docker containers. You need:
- **Docker Desktop** running
- Permissions to create containers

Supported containers: PostgreSQL, MongoDB, Redis, SQL Server

## 💡 Tips

- **Inherit from `IntegrationTestBase`** for automatic access to `Client`, `DbContext`, `Repository`
- **Use `ITestDataSeeder`** for consistent test data
- **Primary constructors (C# 12)**: `public class MyTests(Factory factory) : IntegrationTestBase(factory)`
- Containers are automatically destroyed after tests finish
- Check `MultiContainerWebApplicationFactory.example.txt` for using multiple databases
- Check `AdvancedSampleTests.example.txt` for advanced scenarios

## 🔧 Example Files

- `SampleTests.cs` - Basic CRUD tests
- `MultiContainerWebApplicationFactory.example.txt` - Multi-database setup
- `AdvancedSampleTests.example.txt` - Advanced scenarios (pagination, transactions, etc.)
