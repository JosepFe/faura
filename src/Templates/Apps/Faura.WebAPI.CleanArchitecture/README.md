# 🏛️ Faura.WebAPI.CleanArchitecture Template

**Modular Monolith** template with Clean Architecture, DDD, and CQRS using .NET 8. Ideal for applications with multiple bounded contexts that can evolve to microservices.

## ✨ Includes

- ✅ Clean Architecture (Domain → Application → Infrastructure → API)
- ✅ CQRS with custom SimpleMediator
- ✅ Result Pattern (Faura.Infrastructure.Result)
- ✅ DDD: Encapsulated entities with factory methods
- ✅ Multi-database (MongoDB, SQL Server, PostgreSQL, MySQL)
- ✅ Complete Sample module as reference
- ✅ Docker Compose for development
- ✅ StyleCop + SonarAnalyzer integrated

## 🏗️ Architecture

```
API Layer (Controllers, Contracts)
    ↓
Application Layer (Commands, Queries, Handlers)
    ↓
Domain Layer (Entities, Enums, Interfaces)
    ↓
Infrastructure Layer (DbContext, Repositories)
```

**Independent modules** with Shared components: each module represents a bounded context with its own layers.

## 🚀 How to Use

### 1. Copy the template
```bash
cp -r src/Templates/Apps/Faura.WebAPI.CleanArchitecture ./MyProject
cd MyProject
```

### 2. Rename the project
Find and replace `Template` with `MyProject` in:
- `.csproj` file names
- Namespaces in `.cs` files
- Project references
- `appsettings.json`

### 3. Configure the database
Edit `appsettings.json`:
```json
{
  "Sample": {
    "MongoDb": {
      "ConnectionString": "mongodb://admin:password@localhost:27017",
      "DatabaseName": "myproject_db"
    }
  }
}
```

### 4. Start MongoDB with Docker
```bash
docker-compose up -d
```

### 5. Run the application
```bash
dotnet run --project Template.API/Template.API.csproj
```

Open https://localhost:5001/swagger

## 📂 Structure

```
Template.API/              # Presentation layer
  ├── Controllers/         # REST endpoints
  ├── Contracts/          # Request/Response DTOs
  ├── Mapping/            # DTO conversions
  └── Bootstrappers/      # DI configuration

Modules/
  ├── Sample/             # Example module
  │   ├── Sample.Domain/
  │   ├── Sample.Application/
  │   └── Sample.Infrastructure/
  └── Shared/             # Shared components
      ├── Shared.Domain/
      ├── Shared.Application/
      └── Shared.Infrastructure/
```

## 🎯 Create a New Module

Use the **Sample** module as reference:

### 1. Create the structure
```bash
mkdir -p Modules/Product/Product.{Domain,Application,Infrastructure}
```

### 2. Domain Layer
```csharp
// Product.Domain/Entities/Product.cs
public class Product : EntityBase
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    
    private Product() { } // EF Core
    
    public static Product Create(string name, decimal price)
    {
        return new Product { Name = name, Price = price, CreatedAt = DateTime.UtcNow };
    }
}
```

### 3. Application Layer
```csharp
// Product.Application/Commands/CreateProductCommand.cs
public record CreateProductCommand(string Name, decimal Price) : IRequest<FauraResult<long>>;

// Product.Application/Handlers/CreateProductHandler.cs
public class CreateProductHandler(IProductRepository repo) 
    : IRequestHandler<CreateProductCommand, FauraResult<long>>
{
    public async Task<FauraResult<long>> Handle(CreateProductCommand cmd)
    {
        var product = Product.Create(cmd.Name, cmd.Price);
        await repo.CreateAsync(product);
        return FauraResult<long>.Success(product.Id);
    }
}
```

### 4. Infrastructure Layer
```csharp
// Product.Infrastructure/Persistence/ProductDbContext.cs
public class ProductDbContext(DbContextOptions<ProductDbContext> options) 
    : DbContextBase(options)
{
    public DbSet<Product> Products { get; set; }
}
```

### 5. Register the module
In `ModulesBootstrapper.cs`:
```csharp
services.AddProductModule(configuration);
```

## 🧪 Sample Module

Includes complete example of:
- ✅ Complete CRUD
- ✅ Encapsulated entities
- ✅ Commands and Queries
- ✅ Repository Pattern
- ✅ MongoDB configured

**Use it as template for your modules.**

## 📝 Conventions

- **Entities**: Private setters + Factory methods + Private constructor
- **CQRS**: Commands (write) and Queries (read)
- **Result Pattern**: `FauraResult<T>.Success()` / `FauraResult<T>.Error()`
- **Namespaces**: `using` after `namespace`

## 🔗 More Information

- `docs/ARCHITECTURE.md` - Detailed architecture
- `docs/COMMANDS.md` - Useful commands
- `docs/AI_GUIDE.md` - Guide for using with AI/Copilot


## 📚 Documentación Adicional

- [Arquitectura del Sistema](./docs/Architecture.md)
- [Convenciones de Código](./docs/CodeConventions.md)
- [Guía de Módulos](./docs/ModuleGuide.md)
- [Faura Framework](../../../README.md)

## 🔧 Configuración Adicional

### StyleCop
El template incluye `stylecop.json` con reglas personalizadas. Personalízalo según tus necesidades.

### Docker
El `docker-compose.yml` incluye MongoDB. Agrega otros servicios según necesites (Redis, RabbitMQ, etc.).

### Health Checks
Por defecto en `/health`. Agrega checks personalizados en `ApiBootstrapper.cs`:
```csharp
builder.Services.AddHealthChecks()
    .AddMongoDb(mongoConnectionString, name: "mongodb");
```

## 📄 Licencia

Este template es parte del framework Faura. Ver [LICENSE](../../../LICENSE) para más detalles.

---

**Creado con** ❤️ **usando Faura Framework**
