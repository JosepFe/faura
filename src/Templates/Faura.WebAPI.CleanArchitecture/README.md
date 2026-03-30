# Faura.WebAPI.CleanArchitecture Template

Este template proporciona una base sólida para crear APIs REST siguiendo los principios de **Clean Architecture**, **DDD** y **Arquitectura Hexagonal** utilizando .NET 8 y el framework Faura.

## 🏗️ Arquitectura

El template implementa un **Monolito Modular** que puede escalar fácilmente, con separación clara de responsabilidades:

```
┌─────────────────────────────────────────────────────────────┐
│                        API Layer                             │
│  (Controllers, Contracts, Mapping, Bootstrappers)           │
└───────────────────────┬─────────────────────────────────────┘
                        │
        ┌───────────────┼───────────────┐
        ▼               ▼               ▼
┌──────────────┐ ┌──────────────┐ ┌──────────────┐
│   Module 1   │ │   Module 2   │ │   Module N   │
│ ┌──────────┐ │ │ ┌──────────┐ │ │ ┌──────────┐ │
│ │ Domain   │ │ │ │ Domain   │ │ │ │ Domain   │ │
│ └────┬─────┘ │ │ └────┬─────┘ │ │ └────┬─────┘ │
│      │       │ │      │       │ │      │       │
│ ┌────▼─────┐ │ │ ┌────▼─────┐ │ │ ┌────▼─────┐ │
│ │Application│ │ │ │Application│ │ │ │Application│ │
│ └────┬─────┘ │ │ └────┬─────┘ │ │ └────┬─────┘ │
│      │       │ │      │       │ │      │       │
│ ┌────▼──────┐│ │ ┌────▼──────┐│ │ ┌────▼──────┐│
│ │Infrastructure││ │Infrastructure││ │Infrastructure││
│ └───────────┘ │ │ └───────────┘ │ │ └───────────┘ │
└──────────────┘ └──────────────┘ └──────────────┘
        │               │               │
        └───────────────┼───────────────┘
                        │
        ┌───────────────▼───────────────┐
        │      Shared Components         │
        │  (Domain, Application, Infra)  │
        └───────────────────────────────┘
```

### Flujo de Capas

```
API (Controllers/Contracts) → Application (Commands/Queries/Handlers) → Domain (Entities/Enums)
                                                                        ↓
                                                            Infrastructure (DbContext/Repositories)
```

## 📁 Estructura del Proyecto

```
Template.API/
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
├── Bootstrappers/
│   ├── ApiBootstrapper.cs          # Configuración API (CORS, Auth, Health)
│   ├── ModulesBootstrapper.cs      # Registro de módulos
│   └── SharedBootstrapper.cs       # Servicios compartidos
├── Controllers/
│   └── Sample/
│       └── SampleItemsController.cs
├── Contracts/
│   └── Sample/
│       └── SampleItems/
│           ├── CreateSampleItemRequest.cs
│           └── CreateSampleItemResponse.cs
└── Mapping/
    └── Sample/
        └── SampleItemMappingExtensions.cs

Modules/
├── Sample/                          # Módulo de ejemplo
│   ├── Sample.Domain/
│   │   ├── Entities/
│   │   │   └── SampleItem.cs       # Entidad con factory methods
│   │   ├── Enums/
│   │   │   └── SampleStatus.cs
│   │   └── Repositories/
│   │       └── ISampleItemRepository.cs
│   ├── Sample.Application/
│   │   ├── Commands/
│   │   │   └── CreateSampleItemCommand.cs
│   │   ├── Queries/
│   │   │   └── GetSampleItemByIdQuery.cs
│   │   ├── Handlers/
│   │   │   ├── CreateSampleItemHandler.cs
│   │   │   └── GetSampleItemByIdHandler.cs
│   │   ├── DTOs/
│   │   │   └── SampleItemDto.cs
│   │   └── DependencyInjection.cs
│   └── Sample.Infrastructure/
│       ├── Persistence/
│       │   ├── SampleDbContext.cs
│       │   ├── Options/
│       │   │   └── SampleMongoDbOptions.cs
│       │   └── EntityConfiguration/
│       │       └── SampleItemConfiguration.cs
│       ├── Repositories/
│       │   └── SampleItemRepository.cs
│       └── DependencyInjection.cs
└── Shared/                          # Componentes compartidos
    ├── Shared.Domain/
    │   ├── Entities/
    │   │   └── EntityBase.cs       # Clase base para entidades
    │   └── Repositories/
    │       └── IEntityRepository.cs
    ├── Shared.Application/
    │   └── SimpleMediator/         # CQRS mediator custom
    │       ├── IMediator.cs
    │       ├── IRequest.cs
    │       └── IRequestHandler.cs
    └── Shared.Infrastructure/
        ├── Persistence/
        │   └── DbContextBase.cs
        └── DependencyInjection.cs
```

## 🚀 Características

- ✅ **Clean Architecture** con separación clara de capas
- ✅ **Modular Monolith** - Módulos independientes escalables
- ✅ **CQRS Pattern** con SimpleMediator custom
- ✅ **Result Pattern** usando Faura.Infrastructure.Result
- ✅ **Entity Encapsulation** con factory methods y private setters
- ✅ **Multi-Database Support** - MongoDB, SQL Server, PostgreSQL, MySQL
- ✅ **Health Checks** configurados
- ✅ **CORS** preconfigurado
- ✅ **Logging** con Faura.Infrastructure.Logger
- ✅ **Docker Support** con docker-compose
- ✅ **StyleCop** y **SonarAnalyzer** integrados

## 🛠️ Tecnologías y Librerías

- **.NET 8**
- **Faura Framework** (Result, Logger, ApiBootstrapper)
- **Entity Framework Core** (Multi-database support)
- **MongoDB.EntityFrameworkCore** (Default)
- **Swashbuckle** (Swagger/OpenAPI)

## 📦 Dependencias Faura

El template utiliza las siguientes librerías de Faura:

- `Faura.Infrastructure.Result` - Result Pattern para manejo robusto de errores
- `Faura.Infrastructure.Logger` - Sistema de logging estructurado
- `Faura.Infrastructure.ApiBoostraper` - Configuración común de APIs
- `Faura.Infrastructure.UnitOfWork` - Soporte multi-database (opcional)

## 🎯 Convenciones de Código

### Namespace/Using Order
```csharp
namespace Template.Sample.Domain.Entities;

using MongoDB.Bson.Serialization.Attributes;
using Template.Shared.Domain.Entities;
```

### Encapsulación de Entidades
- **Private setters** para todas las propiedades
- **Factory methods estáticos** para creación (`Create()`, `CreateSystem()`, etc.)
- **Métodos públicos** para mutaciones con validación
- **Constructor privado**

### Patrón CQRS
- **Commands** para operaciones de escritura (retornan `FauraResult<T>`)
- **Queries** para operaciones de lectura (retornan `FauraResult<T>`)
- **Handlers** implementan la lógica de negocio

### Result Pattern
```csharp
// En Handler
return FauraResult<SampleItemDto>.Success(dto);
return FauraResult<SampleItemDto>.Error(new FauraError("CODE", "Message"));

// En Controller
var result = await _mediator.Send(command, cancellationToken);
if (!result.IsSuccess)
{
    return result.BuildResult(); // Convierte errores a HTTP responses
}
return FauraResult<Response>.Success(response).BuildResult(HttpStatusCode.Created);
```

## 📝 Cómo Usar Este Template

### 1. Renombrar el Proyecto

Reemplaza `Template` por el nombre de tu proyecto en:
- Nombres de archivos `.csproj`
- Namespaces en archivos `.cs`
- Referencias de proyecto
- `appsettings.json` (ApplicationName en Logging)

### 2. Agregar un Nuevo Módulo

Sigue el patrón del módulo Sample:

1. **Crear estructura de carpetas**:
   ```
   Modules/YourModule/
   ├── YourModule.Domain/
   ├── YourModule.Application/
   └── YourModule.Infrastructure/
   ```

2. **Domain Layer**:
   - Entidades con factory methods
   - Enums
   - Interfaces de repositorios

3. **Application Layer**:
   - Commands y Queries
   - Handlers
   - DTOs
   - `DependencyInjection.cs` para registrar handlers

4. **Infrastructure Layer**:
   - DbContext
   - Implementación de repositorios
   - `DependencyInjection.cs` para configurar BD y registrar repos

5. **Registrar en API**:
   - Agregar en `ModulesBootstrapper.cs`
   - Crear Contracts en `Contracts/YourModule/`
   - Crear Mapping en `Mapping/YourModule/`
   - Crear Controller en `Controllers/YourModule/`
   - Agregar configuración MongoDB en `appsettings.json`

### 3. Configurar Base de Datos

El template soporta múltiples bases de datos. Por defecto usa MongoDB.

#### MongoDB (Default)
```json
{
  "Sample": {
    "MongoDb": {
      "ConnectionString": "mongodb://admin:password@localhost:27017",
      "DatabaseName": "sample_db"
    }
  }
}
```

#### SQL Server / PostgreSQL / MySQL
Modifica `DependencyInjection.cs` en Infrastructure:
```csharp
services.AddDbContext<SampleDbContext>(options =>
{
    options.UseSqlServer(connectionString); // o UseNpgsql, UseMySql
});
```

### 4. Ejecutar el Proyecto

```bash
# Iniciar MongoDB
docker-compose up -d

# Ejecutar API
dotnet run --project Template.API/Template.API.csproj

# Build completo
dotnet build

# Ejecutar tests
dotnet test
```

## 🧪 Módulo Sample

El módulo **Sample** incluye un ejemplo completo de CRUD con:

- ✅ Entidad `SampleItem` con factory methods
- ✅ Commands: `CreateSampleItemCommand`, `UpdateSampleItemCommand`
- ✅ Queries: `GetSampleItemByIdQuery`, `GetSampleItemsQuery`
- ✅ Handlers implementados
- ✅ Repository pattern completo
- ✅ DbContext con EntityConfiguration
- ✅ Controller con endpoints REST
- ✅ Contracts (Request/Response)
- ✅ Mapping extensions

**Puedes usar este módulo como referencia para crear nuevos módulos.**

## 🤖 Uso con IA (GitHub Copilot / Claude)

Este template está diseñado para ser usado con asistentes de IA. El módulo Sample proporciona un ejemplo completo que la IA puede usar como referencia para:

1. **Crear nuevos módulos** siguiendo el mismo patrón
2. **Generar entidades** con factory methods y encapsulación
3. **Implementar CQRS** con commands, queries y handlers
4. **Configurar nuevos bounded contexts**

Ejemplo de prompt:
```
"Crea un nuevo módulo llamado Product siguiendo el patrón del módulo Sample. 
La entidad Product debe tener Name, Description, Price y Stock."
```

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
