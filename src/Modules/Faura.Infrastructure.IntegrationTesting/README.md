# 🧪 Faura.Infrastructure.IntegrationTesting

This package is part of the **Faura.Infrastructure** framework, which provides reusable components for enterprise .NET solutions.

`IntegrationTesting` focuses on enabling fast and isolated integration tests using Testcontainers and dependency overrides for database and service mocking.

---

## ✨ Features

- 🐳 Containerized databases for reliable isolated tests
- 🧱 Extendable `WebApplicationFactory` base
- 🌱 Pluggable test data seeders
- 🛠 Custom service configuration
- 🔧 JSON-based setup with `appsettings.Test.json`

---

## 🧬 Supported Containers

| Container    | Configuration Class                  | Default Image       |
|--------------|---------------------------------------|----------------------|
| PostgreSQL   | `PostgresContainerConfiguration`      | `postgres:15-alpine` |
| MongoDB      | `MongoContainerConfiguration`         | `mongo:6`            |
| Redis        | `RedisContainerConfiguration`         | `redis:7`            |
| SQL Server   | `SqlServerContainerConfiguration`     | `mcr.microsoft.com/mssql/server:2022-latest` |

---

## 🔧 How to Extend `CustomWebApplicationFactory`

Your `CustomWebApplicationFactory<T>` inherits from `BaseWebApplicationFactory<T>`. You can override the following methods to configure your environment.

### ✅ `ConfigureTestContainersAsync`

Start Docker containers and return an updated configuration with in-memory overrides such as connection strings.

```csharp
protected override async Task<IConfiguration> ConfigureTestContainersAsync(IConfiguration configuration)
{
    var containerOptions = configuration.GetSection("Containers").Get<TestContainerOptions>();
    var pgConfig = new PostgresContainerConfiguration(containerOptions.Postgres);
    var container = new TestContainerInstance<PostgresContainerConfiguration>(pgConfig);

    await container.StartAsync();

    return new ConfigurationBuilder()
        .AddConfiguration(configuration)
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["ConnectionStrings:MyDb"] = container.ConnectionString
        })
        .Build();
}
```

### ✅ `ConfigureTestDatabase`

Replace the default `DbContext` registration with one using the connection string returned from the container.

```csharp
protected override void ConfigureTestDatabase(IServiceCollection services, IConfiguration configuration)
{
    services.RemoveAll(typeof(DbContextOptions<MyDbContext>));
    services.ConfigureDatabase<MyDbContext>(
        configuration.GetConnectionString("MyDb")!,
        DatabaseType.PostgreSQL,
        ServiceLifetime.Scoped);
}
```

### ✅ `ConfigureTestServices`

Register mocked services using your preferred mocking library (e.g., NSubstitute or Moq).

```csharp
protected override void ConfigureTestServices(IServiceCollection services, IConfiguration configuration)
{
    var fakeService = Substitute.For<IExternalService>();
    fakeService.GetData().Returns("mocked value");

    services.AddSingleton(fakeService);
}
```

---

## 🌱 Test Data Seeding

Extend the `TestDataSeeder<TContext>` abstract class. It will ensure the database is created and provide a scoped `DbContext` for seeding.

```csharp
public class MySeeder : TestDataSeeder<MyDbContext>
{
    protected override async Task SeedDataAsync(MyDbContext context, IServiceProvider scopedProvider)
    {
        context.Users.Add(new User("Alice"));
        await context.SaveChangesAsync();
    }
}
```

Register it like this in your factory:

```csharp
services.AddScoped<ITestDataSeeder, MySeeder>();
```

It will be automatically resolved and run during test factory initialization.

---

## 🔧 appsettings.Test.json Example

```json
{
  "Containers": {
    "Postgres": {
      "Image": "postgres:15-alpine",
      "Port": 5432,
      "Username": "postgres",
      "Password": "postgres",
      "Database": "TestDb"
    }
  }
}
```

---

## ✅ Requirements

- .NET 8
- Docker running locally
- Testcontainers for .NET

---

## 📄 License

This project is licensed under the Apache License 2.0.

You may use, distribute, and modify this software under the terms of the license.
It permits usage in proprietary projects as long as the original copyright and license
notice are retained in all copies or substantial portions of the software.

---

## 🤝 Contributions

You’re welcome to fork, extend, or suggest new container configs!