# 🚀 Faura.WebAPI Template

This template provides a **solid foundation** for creating REST APIs using .NET 8, following best practices with **Repository Pattern**, **Unit of Work**, and **Dependency Injection**.

## 📋 Overview

This is a **reusable template** - copy this structure into your project and adapt it to your needs. The template includes a complete **Sample** entity implementation as a reference.

### ✨ What's Included

- ✅ **Sample Entity** with full CRUD operations
- ✅ **Repository Pattern** using Faura.Infrastructure.UnitOfWork
- ✅ **Service Layer** with business logic separation
- ✅ **REST API Controllers** with proper HTTP status codes
- ✅ **Unit of Work** for transaction management
- ✅ **Logging** with Faura.Infrastructure.Logger
- ✅ **Swagger** with authentication options
- ✅ **Database migrations** with Entity Framework Core
- ✅ **Dependency Injection** setup with bootstrappers

---

## 🏗️ Project Structure

```
Faura.WebAPI/
├── Application/
│   ├── ISampleService.cs              # Service interface
│   └── SampleService.cs                # Service implementation
├── Bootstrappers/
│   ├── ApiBootstrapper.cs              # API configuration (CORS, Swagger, etc.)
│   ├── ApplicationBootstrapper.cs      # Application services registration
│   ├── InfrastructureBootstrapper.cs   # Database and repositories registration
│   └── OptionsBootstrapper.cs          # Configuration options
├── Controllers/
│   └── SamplesController.cs            # REST API endpoints
├── Domain/
│   ├── Entities/
│   │   └── Sample.cs                   # Sample entity
│   ├── ISampleRepository.cs            # Repository interface
│   └── SampleRepository.cs             # Repository implementation
├── Infrastructure/
│   └── Persistence/
│       ├── SampleDbContext.cs          # EF Core DbContext
│       ├── ISampleUoW.cs               # Unit of Work interface
│       └── SampleUoW.cs                # Unit of Work implementation
├── appsettings.json                     # Configuration
├── appsettings.Development.json
└── Program.cs                           # Application entry point
```

---

## 🚀 Getting Started

### 1. Copy Template

```bash
# Copy this template to your project
cp -r Faura.WebAPI YourProject.API
```

### 2. Rename Namespaces

Find and replace `Faura.WebAPI` with `YourProject.API` throughout the project.

### 3. Configure Database

Edit `appsettings.json` and update the connection string:

```json
{
  "ConnectionStrings": {
    "Sample": "Host=localhost;Database=yourdb;Username=user;Password=pass"
  }
}
```

### 4. Run Migrations

```bash
# Create initial migration
dotnet ef migrations add InitialCreate --context SampleDbContext

# Apply migrations
dotnet ef database update --context SampleDbContext
```

### 5. Run the API

```bash
dotnet run
```

The API will be available at `https://localhost:5001` (or the port configured in `launchSettings.json`).

---

## 📚 Sample Entity Usage

The template includes a complete **Sample** entity as a reference. Here's how it's structured:

### Entity (`Domain/Entities/Sample.cs`)

```csharp
public class Sample
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### Repository (`Domain/SampleRepository.cs`)

Uses `EntityRepository<T>` base class from Faura.Infrastructure.UnitOfWork:

```csharp
public class SampleRepository : EntityRepository<Sample>, ISampleRepository
{
    public SampleRepository(
        SampleDbContext dbContext,
        ILogger<EntityRepository<Sample>> logger,
        bool enableTracking = false
    ) : base(dbContext, logger, enableTracking) { }
}
```

### Service (`Application/SampleService.cs`)

Business logic layer:

```csharp
public class SampleService : ISampleService
{
    private readonly ISampleRepository _sampleRepository;
    private readonly ISampleUoW _uoW;
    
    public async Task<Sample> CreateSampleAsync(string name, string description, string category)
    {
        var sample = new Sample(name, description, category);
        return await _sampleRepository.CreateAsync(sample);
    }
    
    // Other CRUD methods...
}
```

### Controller (`Controllers/SamplesController.cs`)

REST API endpoints:

```csharp
[ApiController]
[Route("api/[controller]")]
public class SamplesController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() { }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id) { }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSampleRequest request) { }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateSampleRequest request) { }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id) { }
}
```

---

## 🎯 API Endpoints

Once running, the following endpoints are available:

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/samples` | Get all samples |
| GET | `/api/samples/{id}` | Get sample by ID |
| POST | `/api/samples` | Create new sample |
| PUT | `/api/samples/{id}` | Update existing sample |
| DELETE | `/api/samples/{id}` | Delete sample |
| POST | `/api/samples/multiple` | Create multiple samples in transaction |

### Example Request

**Create Sample:**
```bash
curl -X POST https://localhost:5001/api/samples \
  -H "Content-Type: application/json" \
  -d '{
    "name": "My Sample",
    "description": "Sample description",
    "category": "CategoryA"
  }'
```

---

## 🔧 Configuration Options

### Swagger Authentication

The template supports multiple authentication methods. Configure in `appsettings.json`:

```json
{
  "Swagger": {
    "Authentication": {
      "Bearer": {
        "Enable": true,
        "Name": "Bearer"
      },
      "ApiKey": {
        "Enable": true,
        "Name": "X-API-Key",
        "In": "Header"
      }
    }
  }
}
```

### Logging

Configure logging outputs in `appsettings.json`:

```json
{
  "Logging": {
    "MinimumLevel": {
      "Default": "Information"
    },
    "ApplicationName": "YourApp",
    "Outputs": {
      "Console": {
        "Enable": true
      }
    }
  }
}
```

---

## 🛠️ Creating Your Own Entities

To add a new entity, follow the Sample pattern:

### 1. Create Entity Class

```csharp
// Domain/Entities/Product.cs
public class Product
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

### 2. Create Repository Interface and Implementation

```csharp
// Domain/IProductRepository.cs
public interface IProductRepository : IEntityRepository<Product> { }

// Domain/ProductRepository.cs
public class ProductRepository : EntityRepository<Product>, IProductRepository
{
    public ProductRepository(
        ProductDbContext dbContext,
        ILogger<EntityRepository<Product>> logger,
        bool enableTracking = false
    ) : base(dbContext, logger, enableTracking) { }
}
```

### 3. Create DbContext

```csharp
// Infrastructure/Persistence/ProductDbContext.cs
public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options)
        : base(options) { }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
        });
    }
}
```

### 4. Create Service

```csharp
// Application/IProductService.cs
public interface IProductService
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product> CreateProductAsync(string name, decimal price);
}

// Application/ProductService.cs
public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    
    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }
    
    // Implementation...
}
```

### 5. Create Controller

```csharp
// Controllers/ProductsController.cs
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    
    public ProductsController(IProductService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _service.GetProductsAsync();
        return Ok(products);
    }
}
```

### 6. Register in Bootstrappers

```csharp
// Bootstrappers/InfrastructureBootstrapper.cs
services.ConfigureDatabase<ProductDbContext>(configuration, "Product", DatabaseType.PostgreSQL);
services.AddScoped<IProductRepository, ProductRepository>();

// Bootstrappers/ApplicationBootstrapper.cs
services.AddScoped<IProductService, ProductService>();
services.AddScoped<IProductUoW, ProductUoW>();
```

### 7. Add Connection String

```json
{
  "ConnectionStrings": {
    "Product": "Host=localhost;Database=products;Username=user;Password=pass"
  }
}
```

---

## 🧪 Testing

This template is designed to work with **Faura.IntegrationTest** template. See the integration test template for examples of testing these endpoints.

---

## 📖 Related Documentation

- [Faura.Infrastructure.UnitOfWork README](../../Modules/Faura.Infrastructure.UnitOfWork/README.md)
- [Faura.Infrastructure.Logger README](../../Modules/Faura.Infrastructure.Logger/README.md)
- [Faura.IntegrationTest Template](../Faura.IntegrationTest/README.md)

---

## 🔄 Migration from Employee to Sample

If you're upgrading from an older version that used Employee, both Employee and Sample coexist for backward compatibility. You can safely:

1. Use **Sample** for new features (recommended)
2. Keep **Employee** for existing code
3. Gradually migrate from Employee to Sample
4. Remove Employee once migration is complete

To remove Employee entities:
1. Delete `EmployeeService.cs`, `IEmployeeService.cs`
2. Delete `EmployeeController.cs`
3. Delete `EmployeeRepository.cs`, `IEmployeeRepository.cs`
4. Delete `EmployeeDbContext.cs`, `EmployeeUoW.cs`, `IEmployeeUoW.cs`
5. Remove Employee registrations from bootstrappers
6. Remove Employee connection string from appsettings.json

---

## 📄 License

This template is part of the Faura Framework. See [LICENSE](../../../LICENSE) for details.

---

**Created with** ❤️ **using Faura Framework**
