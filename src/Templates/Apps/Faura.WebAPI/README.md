# 🚀 Faura.WebAPI Template

Base template for creating REST APIs with .NET 8, Repository Pattern, Unit of Work, and EF Core. Includes a complete example with the **Sample** entity.

## ✨ Includes

- ✅ Entity Framework Core with PostgreSQL
- ✅ Repository Pattern + Unit of Work (Faura.Infrastructure.UnitOfWork)
- ✅ REST API with Swagger
- ✅ Structured logging (Serilog)
- ✅ Dependency Injection organized with Bootstrappers
- ✅ Complete example: Sample entity with CRUD

## 🚀 How to Use

### 1. Copy the template
```bash
cp -r src/Templates/Apps/Faura.WebAPI ./MyProject.API
cd MyProject.API
```

### 2. Rename namespaces
Find and replace `Faura.WebAPI` with `MyProject.API` in all files.

### 3. Configure the database
Edit `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "Sample": "Host=localhost;Database=mydb;Username=user;Password=pass"
  }
}
```

### 4. Create and apply migrations
```bash
dotnet ef migrations add InitialCreate --context SampleDbContext
dotnet ef database update --context SampleDbContext
```

### 5. Run the application
```bash
dotnet run
```

Open https://localhost:5001/swagger to explore the API.

## 📂 Structure

```
Application/          # Business services
Bootstrappers/        # DI configuration
Controllers/          # REST API endpoints
Domain/               # Entities, repositories
Infrastructure/       # DbContext, UnitOfWork
```

## 🔄 Replace Sample with your entity

1. Rename `Sample.cs` → `YourEntity.cs`
2. Update `ISampleRepository` → `IYourEntityRepository`
3. Update `SampleService` → `YourEntityService`
4. Update `SamplesController` → `YourEntitiesController`
5. Update `SampleDbContext` and `SampleUoW`
6. Register your services in `ApplicationBootstrapper` and `InfrastructureBootstrapper`

## 📝 Available Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/samples` | Get all |
| GET | `/api/samples/{id}` | Get by ID |
| POST | `/api/samples` | Create |
| PUT | `/api/samples/{id}` | Update |
| DELETE | `/api/samples/{id}` | Delete |

**Example request:**
```bash
curl -X POST https://localhost:5001/api/samples \
  -H "Content-Type: application/json" \
  -d '{"name": "Test", "description": "Sample description", "category": "General"}'
```
