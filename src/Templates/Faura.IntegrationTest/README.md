# 🧪 Faura.IntegrationTest Template

This template provides a **solid foundation** for creating integration tests in your .NET 8 applications using **Testcontainers**, **xUnit**, and the **Faura.Infrastructure.IntegrationTesting** framework.

## 📋 Overview

This is a **reusable template** - not a standalone project. Copy this structure into your solution and adapt it to your needs.

### ✨ What's Included

- ✅ **CustomWebApplicationFactory** with Testcontainers support
- ✅ **IntegrationTestBase** with C# 12 features (primary constructors)
- ✅ **Complete test examples** (basic + advanced)
- ✅ **Multi-container examples** (PostgreSQL, MongoDB, Redis, SQL Server)
- ✅ **Data seeders** for test setup
- ✅ **FauraResult pattern** integration
- ✅ **xUnit** with modern test organization

---

## 🏗️ Template Structure

```
Faura.IntegrationTest/
├── Configuration/
│   ├── CustomWebApplicationFactory.cs           # Your test factory
│   ├── IntegrationTestBase.cs                   # Base class for tests
│   └── MultiContainerWebApplicationFactory.example.cs  # Multi-container example
├── Seeders/
│   └── SampleTestDataSeeder.cs                  # Test data seeders
├── UseCases/
│   ├── SampleTests.cs                            # Basic CRUD tests
│   └── AdvancedSampleTests.example.txt           # Advanced test examples
├── appsettings.Test.json                         # Test configuration
└── Faura.IntegrationTest.csproj                  # Project file
```

---

## 🚀 Getting Started

### 1. Copy Template to Your Solution

```bash
# Copy the template folder
cp -r src/Templates/Faura.IntegrationTest src/YourProject.IntegrationTest

# Add to your solution
dotnet sln add src/YourProject.IntegrationTest/YourProject.IntegrationTest.csproj
```

### 2. Update Project References

Edit `YourProject.IntegrationTest.csproj`:

```xml
<ItemGroup>
    <ProjectReference Include="..\..\Modules\Faura.Infrastructure.IntegrationTesting\Faura.Infrastructure.IntegrationTesting.csproj" />
    <ProjectReference Include="..\YourProject.API\YourProject.API.csproj" />
    <!-- Add other project references as needed -->
</ItemGroup>
```

### 3. Configure CustomWebApplicationFactory

Update `CustomWebApplicationFactory.cs` to match your application:

```csharp
public class CustomWebApplicationFactory : BaseWebApplicationFactory<Program>
{
    protected override async Task<IConfiguration> ConfigureTestContainersAsync(
        IConfiguration configuration)
    {
        // Configure your containers (PostgreSQL, MongoDB, Redis, etc.)
        var containerOptions = configuration.GetSection("Containers").Get<TestContainerOptions>();
        
        var pgConfig = new PostgresContainerConfiguration(containerOptions!.Postgres);
        var container = new TestContainerInstance<PostgresContainerConfiguration>(pgConfig);
        await container.StartAsync();

        return new ConfigurationBuilder()
            .AddConfiguration(configuration)
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:YourDb"] = container.ConnectionString,
            })
            .Build();
    }

    protected override void ConfigureTestServices(IServiceCollection services, IConfiguration configuration)
    {
        // Register test seeders
        services.AddScoped<ITestDataSeeder, YourTestDataSeeder>();
        
        // Override services for testing (mocks, test doubles)
        // services.RemoveAll<IEmailService>();
        // services.AddScoped<IEmailService, FakeEmailService>();
    }

    protected override void ConfigureTestDatabase(IServiceCollection services, IConfiguration configuration)
    {
        // Configure your DbContext
        services.RemoveAll(typeof(DbContextOptions<YourDbContext>));
        services.ConfigureDatabase<YourDbContext>(
            configuration.GetConnectionString("YourDb")!,
            DatabaseType.PostgreSQL,
            ServiceLifetime.Scoped
        );
    }
}
```

### 4. Configure appsettings.Test.json

```json
{
  "Logging": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    }
  },
  "Containers": {
    "Postgres": {
      "Image": "postgres:15-alpine",
      "Port": 5432,
      "Username": "testuser",
      "Password": "testpass",
      "Database": "testdb"
    }
  }
}
```

### 5. Create Your Test Base Class

```csharp
public abstract class IntegrationTestBase(CustomWebApplicationFactory factory) 
    : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    protected HttpClient Client { get; } = factory.CreateClient();
    protected IServiceScope Scope { get; } = factory.Services.CreateScope();
    protected YourDbContext DbContext { get; } = 
        factory.Services.CreateScope().ServiceProvider.GetRequiredService<YourDbContext>();

    public async Task InitializeAsync()
    {
        await DbContext.Database.EnsureCreatedAsync();
        await SeedTestDataAsync();
    }

    protected virtual Task SeedTestDataAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
        Scope?.Dispose();
        Client?.Dispose();
    }
}
```

### 6. Write Your Tests

```csharp
public class ProductTests(CustomWebApplicationFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetAll_ShouldReturnProducts()
    {
        // Arrange
        await SeedProduct("Test Product");

        // Act
        var response = await Client.GetAsync("/api/products");

        // Assert
        response.EnsureSuccessStatusCode();
        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.NotNull(products);
        Assert.NotEmpty(products);
    }

    private async Task SeedProduct(string name)
    {
        var product = new Product { Name = name };
        await DbContext.Products.AddAsync(product);
        await DbContext.SaveChangesAsync();
    }
}
```

---

## 📚 Examples Included

### Basic CRUD Tests ([SampleTests.cs](UseCases/SampleTests.cs))

- GET all entities
- GET by ID
- POST create
- PUT update
- DELETE
- Validation tests
- Transaction tests

### Advanced Tests ([AdvancedSampleTests.example.txt](UseCases/AdvancedSampleTests.example.txt))

- Pagination
- Filtering and search
- FauraResult pattern testing
- Error handling scenarios
- Concurrent operations
- Database state verification
- Direct repository access

### Multi-Container Setup ([MultiContainerWebApplicationFactory.example.cs](Configuration/MultiContainerWebApplicationFactory.example.cs))

Example showing how to configure:
- PostgreSQL
- MongoDB
- Redis
- SQL Server

All running simultaneously in Docker containers.

---

## 🐳 Testcontainers

This template uses [Testcontainers](https://dotnet.testcontainers.org/) via `Faura.Infrastructure.IntegrationTesting` to provide isolated, disposable database instances for each test run.

### Supported Containers

| Database     | Configuration Class             | Default Image                         |
|--------------|----------------------------------|---------------------------------------|
| PostgreSQL   | `PostgresContainerConfiguration` | `postgres:15-alpine`                  |
| MongoDB      | `MongoContainerConfiguration`    | `mongo:6`                             |
| Redis        | `RedisContainerConfiguration`    | `redis:7-alpine`                      |
| SQL Server   | `SqlServerContainerConfiguration`| `mcr.microsoft.com/mssql/server:2022` |

### Why Testcontainers?

✅ **Isolation**: Each test run gets fresh databases  
✅ **Consistency**: Same environment locally and in CI  
✅ **Speed**: Containers start in seconds  
✅ **Cleanup**: Automatic disposal after tests  

---

## 🎯 Best Practices

### 1. Use C# 12 Features

**Primary Constructors**:
```csharp
// ✅ Modern
public class MyTests(CustomWebApplicationFactory factory) : IntegrationTestBase(factory)
{
    // factory is available directly
}

// ❌ Old style
public class MyTests : IntegrationTestBase
{
    public MyTests(CustomWebApplicationFactory factory) : base(factory) { }
}
```

**Collection Expressions**:
```csharp
// ✅ Modern
var items = [];
var numbers = [1, 2, 3];

// ❌ Old
var items = new List<Item>();
```

### 2. Test Organization

**Organize by HTTP verb**:
```csharp
#region GET Tests
[Fact] public async Task GetAll_Should...() { }
[Fact] public async Task GetById_Should...() { }
#endregion

#region POST Tests
[Fact] public async Task Create_Should...() { }
#endregion
```

### 3. AAA Pattern (Arrange-Act-Assert)

```csharp
[Fact]
public async Task Create_ShouldCreateProduct()
{
    // Arrange
    var request = new CreateProductRequest("Test", 10.99m);
    
    // Act
    var response = await Client.PostAsJsonAsync("/api/products", request);
    
    // Assert
    response.EnsureSuccessStatusCode();
    var product = await response.Content.ReadFromJsonAsync<Product>();
    Assert.NotNull(product);
    Assert.Equal("Test", product.Name);
}
```

### 4. Seed Data Strategies

**Option 1: Global Seeder** (runs once per factory)
```csharp
public class GlobalTestDataSeeder : TestDataSeeder<YourDbContext>
{
    protected override async Task SeedDataAsync(YourDbContext context, IServiceProvider provider)
    {
        // Seed data shared across all tests
    }
}
```

**Option 2: Per-Test Seeding** (runs before each test)
```csharp
protected override async Task SeedTestDataAsync()
{
    // Seed data specific to this test class
    await Repository.CreateAsync(new Entity(...));
}
```

**Option 3: In-Test Seeding** (explicit per test)
```csharp
[Fact]
public async Task Test_WithSpecificData()
{
    // Arrange - seed inline
    await DbContext.Products.AddAsync(new Product("Test"));
    await DbContext.SaveChangesAsync();
    
    // Act & Assert...
}
```

### 5. Test Isolation

Each test should be **independent**:
- ✅ Don't rely on test execution order
- ✅ Seed required data in the test or setup
- ✅ Clean up is automatic (database recreated per test)

### 6. Testing FauraResult Pattern

```csharp
[Fact]
public async Task ShouldReturnFauraSuccess()
{
    var response = await Client.PostAsJsonAsync("/api/products", request);
    response.EnsureSuccessStatusCode();
    
    var result = await response.Content.ReadFromJsonAsync<FauraResult<Product>>();
    Assert.True(result.IsSuccess);
    Assert.NotNull(result.Data);
    Assert.Null(result.Error);
}

[Fact]
public async Task ShouldReturnFauraError_WhenValidationFails()
{
    var response = await Client.PostAsJsonAsync("/api/products", invalidRequest);
    
    var result = await response.Content.ReadFromJsonAsync<FauraResult<Product>>();
    Assert.False(result.IsSuccess);
    Assert.Null(result.Data);
    Assert.NotNull(result.Error);
    Assert.Equal("VALIDATION_ERROR", result.Error.Code);
}
```

---

## 🧪 Running Tests

### Locally

```bash
# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "FullyQualifiedName~SampleTests"

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run with coverage
dotnet test /p:CollectCoverage=true /p:CoverageReportsDirectory=./coverage
```

### CI/CD

Ensure Docker is available in your CI pipeline:

**GitHub Actions**:
```yaml
- name: Run Integration Tests
  run: dotnet test --configuration Release
  env:
    DOCKER_HOST: unix:///var/run/docker.sock
```

**Azure DevOps**:
```yaml
- task: DotNetCoreCLI@2
  inputs:
    command: 'test'
    projects: '**/*IntegrationTest.csproj'
```

---

## 🔧 Advanced Scenarios

### Multiple Databases

```csharp
public class MultiDbIntegrationTestBase(CustomWebApplicationFactory factory)
    : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    protected ProductDbContext ProductDb { get; } = 
        factory.Services.CreateScope().ServiceProvider.GetRequiredService<ProductDbContext>();
    
    protected OrderDbContext OrderDb { get; } = 
        factory.Services.CreateScope().ServiceProvider.GetRequiredService<OrderDbContext>();
}
```

### Testing with Authentication

```csharp
[Fact]
public async Task AuthenticatedRequest_ShouldSucceed()
{
    // Arrange
    var token = await GetAuthTokenAsync("testuser", "password");
    Client.DefaultRequestHeaders.Authorization = 
        new AuthenticationHeaderValue("Bearer", token);
    
    // Act
    var response = await Client.GetAsync("/api/protected-resource");
    
    // Assert
    response.EnsureSuccessStatusCode();
}

private async Task<string> GetAuthTokenAsync(string username, string password)
{
    var loginRequest = new { username, password };
    var response = await Client.PostAsJsonAsync("/api/auth/login", loginRequest);
    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
    return result.Token;
}
```

### Mocking External Services

```csharp
protected override void ConfigureTestServices(IServiceCollection services, IConfiguration configuration)
{
    // Remove real email service
    services.RemoveAll<IEmailService>();
    
    // Add fake implementation
    services.AddScoped<IEmailService, FakeEmailService>();
}

public class FakeEmailService : IEmailService
{
    public List<string> SentEmails { get; } = [];
    
    public Task SendEmailAsync(string to, string subject, string body)
    {
        SentEmails.Add(to);
        return Task.CompletedTask;
    }
}
```

---

## 📖 Related Documentation

- [Faura.Infrastructure.IntegrationTesting README](../../Modules/Faura.Infrastructure.IntegrationTesting/README.md)
- [Testcontainers for .NET](https://dotnet.testcontainers.org/)
- [xUnit Documentation](https://xunit.net/)

---

## 🐛 Troubleshooting

### Docker Not Available

```
Error: Docker daemon is not running
```

**Solution**: Ensure Docker Desktop is running or Docker daemon is started.

### Port Conflicts

```
Error: Port 5432 already in use
```

**Solution**: Change the port in `appsettings.Test.json`:
```json
"Postgres": {
  "Port": 5433  // Use different port
}
```

### Container Startup Timeout

```
Error: Container failed to start within timeout
```

**Solution**: Increase timeout or check Docker resources (CPU/Memory).

---

## 📄 License

This template is part of the Faura Framework. See [LICENSE](../../../LICENSE) for details.

---

**Created with** ❤️ **using Faura.Infrastructure.IntegrationTesting**
