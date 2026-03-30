# AI Usage Guide

This template is designed to work seamlessly with AI assistants (GitHub Copilot, Claude, ChatGPT, etc.) to accelerate development.

## Modern C# Features Used

This template uses the latest C# 12 and .NET 8 features for maximum code quality:

### Primary Constructors
All handlers, repositories, controllers, and services use primary constructors:

```csharp
// ✅ Modern approach
public class CreateSampleItemHandler(
    ISampleItemRepository repository,
    ILogger<CreateSampleItemHandler> logger) 
    : IRequestHandler<CreateSampleItemCommand, FauraResult<SampleItemDto>>
{
    // Use 'repository' and 'logger' directly - no need for fields
}

// ❌ Old approach - DON'T use this
public class CreateSampleItemHandler : IRequestHandler<...>
{
    private readonly ISampleItemRepository _repository;
    private readonly ILogger _logger;
    
    public CreateSampleItemHandler(ISampleItemRepository repository, ILogger logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
```

### Collection Expressions
Use collection expressions (`[]`) instead of `new List<>()`:

```csharp
// ✅ Modern
public List<string> Tags { get; private set; } = [];

// ❌ Old - DON'T use
public List<string> Tags { get; private set; } = new();
public List<string> Tags { get; private set; } = new List<string>();
```

### Required Members
Use `required` keyword for non-nullable properties in options classes:

```csharp
// ✅ Modern
public class SampleMongoDbOptions
{
    public required MongoDbSettings MongoDb { get; set; }
}

// ❌ Old - DON'T use
public MongoDbSettings? MongoDb { get; set; }
```

### ArgumentNullException.ThrowIfNull
Use modern null checking:

```csharp
// ✅ Modern
ArgumentNullException.ThrowIfNull(request);

// ❌ Old - DON'T use
if (request == null)
    throw new ArgumentNullException(nameof(request));
```

### Global Usings
Each project has a `GlobalUsings.cs` file with common imports. Don't repeat these in individual files.

---

## How to Use This Template with AI

### 1. The Sample Module as Reference

The **Sample module** provides a complete, working example that AI can analyze and replicate. When asking AI to create new modules, always reference it:

```
"Create a new module called Product following the exact pattern of the Sample module. 
The Product entity should have Name, Description, Price, and Stock properties."
```

### 2. Module Generation Pattern

Use this prompt structure for creating new modules:

```
Create a module named [ModuleName] following the Sample module pattern with:

Domain Layer:
- Entity: [EntityName] with properties [list properties]
- Enums: [list enums if needed]
- Repository interface: I[EntityName]Repository

Application Layer:
- Commands: Create, Update, Delete
- Queries: GetById, GetAll (paginated)
- DTOs: [EntityName]Dto
- Handlers for all commands and queries

Infrastructure Layer:
- DbContext: [ModuleName]DbContext
- Repository implementation
- Entity configuration
- MongoDB options class

API Layer:
- Controller: [EntityName]sController
- Contracts: Requests and Responses
- Mapping extensions

Register the module in ModulesBootstrapper.cs and add MongoDB config to appsettings.json
```

### 3. Entity Creation Pattern

For creating entities, use this pattern:

```
Create an entity named [EntityName] in [Module].Domain.Entities with:
- Properties: [list with types]
- Collection properties initialized with [] (collection expressions)
- Factory method: Create[EntityName]([parameters])
- Mutation methods: [list methods like Update, ChangeStatus, etc.]
- Private setters for all properties
- Validation in factory and mutation methods
- Inherits from EntityBase

Example:
public class [EntityName] : EntityBase
{
    private [EntityName]() { }
    
    public string Name { get; private set; } = null!;
    public List<string> Tags { get; private set; } = []; // Collection expression
    
    public static [EntityName] Create(string name, ...)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name required", nameof(name));
            
        return new [EntityName] { Name = name, ... };
    }
}
```

### 4. CQRS Implementation Pattern

```
Implement CQRS for [Entity]:

Commands:
- Create[Entity]Command with handler
- Update[Entity]Command with handler
- Delete[Entity]Command with handler

Queries:
- Get[Entity]ByIdQuery with handler
- Get[Entity]sQuery with handler (paginated)

All handlers should:
- Use primary constructors (C# 12)
- Return FauraResult<T>
- Use repository interfaces
- Include logging with ILogger
- Validate input
- Handle errors gracefully

Example:
public class Create[Entity]Handler(
    I[Entity]Repository repository,
    ILogger<Create[Entity]Handler> logger)
    : IRequestHandler<Create[Entity]Command, FauraResult<[Entity]Dto>>
{
    // Use 'repository' and 'logger' directly
}
```

### 5. Repository Pattern

```
Create repository for [Entity]:

Interface (in Domain):
- GetByIdAsync
- GetAllAsync (with pagination and filters)
- AddAsync
- UpdateAsync
- DeleteAsync
- Custom query methods as needed

Implementation (in Infrastructure):
- Use primary constructor with DbContext parameter
- Implement all interface methods
- Apply filters in GetAllAsync
- Use AsQueryable for complex queries

Example:
public class [Entity]Repository(YourDbContext context) : I[Entity]Repository
{
    // Use 'context' directly throughout the class
    public async Task<[Entity]?> GetByIdAsync(string id, CancellationToken ct)
    {
        return await context.[Entities].FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}
```

### 6. API Endpoints Pattern

```
Create REST endpoints for [Entity]:

- POST /api/[entities] - Create
- GET /api/[entities]/{id} - Get by ID
- GET /api/[entities] - Get all (paginated with filters)
- PUT /api/[entities]/{id} - Update
- DELETE /api/[entities]/{id} - Delete

Include:
- Request/Response contracts
- Mapping extensions
- Controller with proper HTTP status codes
- XML documentation comments
```

## Example Prompts

### Creating a Complete Module

```
Create a Product module following the Sample module pattern:

Entity Properties:
- Name (string, required)
- Description (string, optional)
- Price (decimal, required, must be positive)
- Stock (int, required, must be non-negative)
- Category (enum: Electronics, Clothing, Food, Other)
- IsAvailable (bool, computed from Stock > 0)

Include:
- Create/Update/Delete commands
- GetById and GetAll queries with filters by category and availability
- Repository with ExistsByNameAsync method
- Controller with full CRUD
- Validation in entity and handlers

Register in ModulesBootstrapper and add to appsettings.json
```

### Adding a Feature to Existing Module

```
Add a new feature to the Sample module:

Feature: Archive functionality
- Add Archive() method to SampleItem entity
- Add ArchiveSampleItemCommand and handler
- Add POST /api/sample-items/{id}/archive endpoint
- Update GetSampleItemsQuery to filter archived items by default
```

### Implementing Relationships

```
Implement a relationship between Product and Category modules:

- Add CategoryId property to Product entity
- Add GetProductsByCategoryId query
- Add GetCategoryWithProducts query
- Handle cascade delete: when category is deleted, what happens to products?
- Add validation: category must exist when creating product
```

## AI Tips & Tricks

### 1. Be Specific About Patterns

✅ Good:
```
"Create CreateProductCommand following the CQRS pattern like CreateSampleItemCommand, 
returning FauraResult<ProductDto>"
```

❌ Bad:
```
"Create a command for products"
```

### 2. Reference Existing Code

Always reference similar code in the template:

```
"Create ProductRepository similar to SampleItemRepository in 
Template.Sample.Infrastructure/Repositories/SampleItemRepository.cs"
```

### 3. Ask for Complete Implementations

```
"Create the complete Product module including all layers (Domain, Application, 
Infrastructure, API) with the same structure as the Sample module"
```

### 4. Validate Patterns

```
"Review the ProductsController and ensure it follows the same pattern as 
SampleItemsController, especially error handling and result mapping"
```

### 5. Incremental Development

Start small and build up:

1. "Create Product entity with factory method"
2. "Add CreateProductCommand and handler"
3. "Add ProductRepository implementation"
4. "Create ProductsController with Create endpoint"
5. "Add remaining CRUD operations"

## Common Patterns to Request

### Enum-based Filtering
```
"Add filtering by [EnumName] in Get[Entity]sQuery, similar to how SampleStatus 
filtering works in GetSampleItemsQuery"
```

### Search Functionality
```
"Add text search by Name and Description in Get[Entity]sQuery using Contains, 
like the SearchTerm filter in GetSampleItemsQuery"
```

### Soft Delete
```
"Implement soft delete for [Entity] by adding IsDeleted property and updating 
all queries to filter IsDeleted = false by default"
```

### Audit Fields
```
"Add audit fields (CreatedBy, UpdatedBy) to [Entity] following EntityBase pattern"
```

## Troubleshooting with AI

If you encounter issues:

```
"I'm getting [error] in [file]. Review the implementation compared to the Sample 
module and suggest fixes."
```

```
"The [Entity] module isn't working. Compare my implementation with SampleItem and 
identify differences."
```

## Advanced AI Usage

### Code Review
```
"Review my Product module implementation and compare it to the Sample module. 
Identify any deviations from the patterns and suggest improvements."
```

### Refactoring
```
"Refactor the Order module to use the same patterns as the Sample module, 
especially for error handling and validation."
```

### Testing
```
"Generate unit tests for CreateProductHandler following the same testing patterns 
that would be used for CreateSampleItemHandler."
```

---

The key to success with AI and this template is **consistency**. Always reference the Sample module as the source of truth for patterns and conventions.
