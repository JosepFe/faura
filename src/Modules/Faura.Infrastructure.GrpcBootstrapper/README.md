# Faura.Infrastructure.GrpcBootstrapper

🚀 A set of extensions and utilities to streamline the setup and configuration of gRPC services in ASP.NET Core applications.

This package encapsulates common practices for projects using gRPC, such as header propagation interceptors, service registration, and environment-specific configuration, all within a modular and extensible architecture.

---

## 📦 Contents

### 🔧 `Extensions/`

- **`ConfigurationExtensions.cs`**  
  Adds secure configuration using `UserSecrets` for `Local` and `Development` environments.

- **`HeadersPropagationExtensions.cs`**  
  Registers propagation of the `x-correlation-id` header between services and clients using `HeaderPropagation`.

- **`WebApplicationBuilderExtensions.cs`**  
  Bootstraps core infrastructure:
  - Registers gRPC interceptors like `CorrelationIdInterceptor`
  - Enables header propagation
  - Adds environment variables and user secrets

- **`WebApplicationExtensions.cs`**  
  Encapsulates middleware configuration and gRPC service registration using `UseEndpoints`.

### 🧩 `Interceptors/`

- **`CorrelationIdInterceptor.cs`**  
  A gRPC interceptor that:
  - Reads or generates a `x-correlation-id`
  - Propagates it through the context (`HeaderPropagationValues`)
  - Adds it as a `ResponseTrailer`

---

## ✅ Usage

### 1. Register common dependencies:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.RegisterSettingsProvider<Program>();
builder.BootstrapCommonFauraServices();
```

> This enables `AddGrpc` with the interceptor and `HeaderPropagation`.

---

### 2. Register gRPC services:

```csharp
public static Action<IEndpointRouteBuilder> RegisterGrpcServices()
{
    return endpoints =>
    {
        endpoints.MapGrpcService<GreeterService>();
    };
}
```

---

### 3. Configure the application:

```csharp
var app = builder.Build();

app.ConfigureCommonFauraWebApplication(RegisterGrpcServices());

app.Run();
```

> This encapsulates `UseRouting`, `UseHeaderPropagation`, and `UseEndpoints`.

---

## 🔗 Propagated Headers

This package supports propagating the following header:

- `x-correlation-id`: used for distributed tracing across gRPC services.

---

## 💡 Requirements

- .NET 8 or higher
- NuGet packages:
  - `Grpc.AspNetCore`
  - `Microsoft.AspNetCore.HeaderPropagation`

---

## 📁 Package Structure

```
Faura.Infrastructure.GrpcBootstrapper/
├── Extensions/
│   ├── ConfigurationExtensions.cs
│   ├── HeadersPropagationExtensions.cs
│   ├── WebApplicationBuilderExtensions.cs
│   └── WebApplicationExtensions.cs
├── Interceptors/
│   └── CorrelationIdInterceptor.cs
├── LICENSE
└── README.md
```

---

## 📜 License

This package is licensed under the terms specified in the included `LICENSE` file.
