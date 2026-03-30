# Clean Architecture Guidelines

## Overview

This template follows Clean Architecture principles with clear separation of concerns across layers.

## Layer Responsibilities

### Domain Layer
- **Purpose**: Core business logic and entities
- **Dependencies**: None (most independent)
- **Contains**:
  - Entities with factory methods
  - Enums
  - Repository interfaces
  - Domain exceptions
  - Value objects

**Rules**:
- No dependencies on other layers
- Business rules are encapsulated in entities
- Use factory methods for entity creation
- Private setters for properties
- Public methods for mutations with validation

**Example**:
```csharp
public class Product : EntityBase
{
    private Product() { }
    
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; }
    
    public static Product Create(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name required");
            
        if (price <= 0)
            throw new ArgumentException("Price must be positive");
            
        return new Product { Name = name, Price = price };
    }
    
    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("Price must be positive");
            
        Price = newPrice;
        MarkAsUpdated();
    }
}
```

### Application Layer
- **Purpose**: Use cases and business workflows
- **Dependencies**: Domain layer only
- **Contains**:
  - Commands and Queries (CQRS)
  - Handlers
  - DTOs
  - Application interfaces

**Rules**:
- Depends only on Domain
- Orchestrates business workflows
- Uses repository interfaces (defined in Domain)
- Returns FauraResult<T> for error handling

**Example**:
```csharp
public class CreateProductHandler : IRequestHandler<CreateProductCommand, FauraResult<ProductDto>>
{
    private readonly IProductRepository _repository;
    
    public async Task<FauraResult<ProductDto>> Handle(
        CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        // Validate
        if (await _repository.ExistsByNameAsync(command.Name))
            return FauraResult<ProductDto>.Error(new FauraError("DUPLICATE", "Product exists"));
        
        // Create entity using factory method
        var product = Product.Create(command.Name, command.Price);
        
        // Save
        await _repository.AddAsync(product, cancellationToken);
        
        // Map to DTO
        var dto = new ProductDto(product.Id, product.Name, product.Price);
        
        return FauraResult<ProductDto>.Success(dto);
    }
}
```

### Infrastructure Layer
- **Purpose**: Technical implementations
- **Dependencies**: Application and Domain layers
- **Contains**:
  - DbContext
  - Repository implementations
  - External service integrations
  - Entity configurations

**Rules**:
- Implements interfaces from Domain/Application
- Handles database, file system, external APIs
- Contains EF Core configurations

**Example**:
```csharp
public class ProductRepository : IProductRepository
{
    private readonly YourDbContext _context;
    
    public async Task<Product?> GetByIdAsync(string id, CancellationToken ct)
    {
        return await _context.Products.FindAsync(new object[] { id }, ct);
    }
    
    public async Task AddAsync(Product product, CancellationToken ct)
    {
        await _context.Products.AddAsync(product, ct);
        await _context.SaveChangesAsync(ct);
    }
}
```

### API Layer
- **Purpose**: HTTP endpoints and API contracts
- **Dependencies**: Application via Mediator
- **Contains**:
  - Controllers
  - Request/Response contracts
  - Mapping extensions
  - Bootstrappers

**Rules**:
- Controllers are thin, just route requests to mediator
- Use contracts (Request/Response) for API surface
- Map between contracts and commands/queries
- Use FauraResult.BuildResult() for HTTP responses

**Example**:
```csharp
[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductRequest request,
        CancellationToken ct)
    {
        var command = request.ToCommand();
        var result = await _mediator.Send(command, ct);
        
        if (!result.IsSuccess)
            return result.BuildResult();
            
        var response = result.Data.ToResponse();
        return FauraResult<CreateProductResponse>
            .Success(response)
            .BuildResult(HttpStatusCode.Created);
    }
}
```

## Dependency Flow

```
API Layer
   ↓ (depends on)
Application Layer
   ↓ (depends on)
Domain Layer
   ↑ (implemented by)
Infrastructure Layer
```

**Key Points**:
- Dependencies point inward (toward Domain)
- Domain has no dependencies
- Infrastructure implements interfaces from Domain
- Application orchestrates using Domain interfaces

## Modular Monolith Pattern

Each module is self-contained with its own:
- Domain (entities specific to the module)
- Application (use cases for the module)
- Infrastructure (database context and repositories)

Modules communicate through:
- Shared contracts
- Events (if needed)
- Direct service calls (for now)

## CQRS Pattern

Separate read and write operations:

**Commands**: Modify state
```csharp
public record CreateProductCommand(...) : IRequest<FauraResult<ProductDto>>;
```

**Queries**: Read state
```csharp
public record GetProductByIdQuery(string Id) : IRequest<FauraResult<ProductDto>>;
```

**Benefits**:
- Clear intent
- Different optimization strategies
- Scalable (can split read/write databases)

## Best Practices

1. **Entity Encapsulation**: Always use factory methods and private setters
2. **Result Pattern**: Return `FauraResult<T>` instead of throwing exceptions
3. **Validation**: Validate in entity methods and handlers
4. **Mapping**: Use explicit mapping extensions (no AutoMapper)
5. **Naming**: Be explicit - `CreateProductCommand`, not `ProductCommand`
6. **Testing**: Each layer can be tested independently
7. **Logging**: Use structured logging with FauraLogger extensions

## Testing Strategy

- **Unit Tests**: Test Domain entities and Application handlers
- **Integration Tests**: Test Infrastructure repositories
- **API Tests**: Test Controllers end-to-end

---

For more details, see the main README.md
