# Faura.Infrastructure.UnitOfWork

Data access library implementing Unit of Work and Repository patterns for Entity Framework Core with multi-database support.

## Features

- **Unit of Work**: Transaction management
- **Generic Repository**: CRUD with filtering, sorting, and pagination
- **Raw SQL**: Execute queries with Dapper
- **Projections**: LINQ transformations
- **Multi-Database**: SQL Server, MySQL, PostgreSQL, SQLite, InMemory

## Installation

```bash
dotnet add package Faura.Infrastructure.UnitOfWork
```

## Quick Start

```csharp
using Faura.Infrastructure.UnitOfWork.Configuration;

// Configure database and register services
services.ConfigureDatabase<MyDbContext>(
    connectionString: "Server=...;Database=...;",
    databaseType: DatabaseType.SqlServer
);
services.AddUnitOfWorkWithRepositories<MyDbContext>();
```

## Usage Examples

### Unit of Work (Transactions)

```csharp
var transaction = await _unitOfWork.GetDbTransaction();
try
{
    await _orderRepo.CreateAsync(order, autoSaveChanges: false);
    await _itemRepo.CreateRangeAsync(items, autoSaveChanges: false);
    await _unitOfWork.CommitTransaction(transaction);
}
catch { throw; }
```

### Entity Repository

```csharp
// CRUD operations
await _repository.CreateAsync(entity);
await _repository.UpdateAsync(entity);
await _repository.DeleteAsync(entity);

// Queries
var items = await _repository.GetAsync(x => x.IsActive);
var sorted = await _repository.GetSortedAsync(x => x.IsActive, x => x.Price, SortDirection.Ascending);
var paged = await _repository.GetPagedAsync(page: 1, pageSize: 20);
var count = await _repository.CountAsync(x => x.IsActive);
```

### Raw SQL Repository

```csharp
var sql = "SELECT * FROM Products WHERE Price > @MinPrice";
var results = await _rawSqlRepo.QueryAsync(sql, new { MinPrice = 100 });
var affected = await _rawSqlRepo.ExecuteAsync("DELETE FROM Products WHERE IsActive = 0");
```

### Projector

```csharp
var dtos = await _projector.GetProjectionAsync(
    projection: query => query
        .Where(p => p.IsActive)
        .Select(p => new ProductDto { Id = p.Id, Name = p.Name })
);
```

## Components

| Component | Use For |
|-----------|---------|
| **EntityRepository** | Standard CRUD with EF Core |
| **RawSqlRepository** | Raw SQL queries and bulk operations |
| **Projector** | DTO mappings and aggregations |
| **UnitOfWork** | Atomic transactions |

## License

See [LICENSE](../../../LICENSE) file.
