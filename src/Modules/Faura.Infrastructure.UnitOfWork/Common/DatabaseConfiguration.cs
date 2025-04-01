namespace Faura.Infrastructure.UnitOfWork.Common;

using Faura.Infrastructure.UnitOfWork.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

public static class DatabaseConfigurator
{
    private const int MaxRetryDelay = 30;
    private const int MaxRetryCount = 10;

    public static void ConfigureDatabase<TContext>(
        this IServiceCollection services,
        string connectionString,
        DatabaseType databaseType,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where TContext : DbContext
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));

        services.AddDbContext<TContext>(options => ConfigureDatabaseOptions(options, databaseType, connectionString), serviceLifetime);
    }

    public static async Task ConfigureDatabase<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionStringName,
        DatabaseType databaseType,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped,
        bool runMigrations = false) where TContext : DbContext
    {
        var connectionString = configuration.GetConnectionString(connectionStringName)
            ?? throw new ArgumentException($"Connection string '{connectionStringName}' not found or is empty.");

        services.AddDbContext<TContext>(options => ConfigureDatabaseOptions(options, databaseType, connectionString), serviceLifetime);

        if (runMigrations)
            await ApplyMigrations<TContext>(services).ConfigureAwait(false);
    }

    private static void ConfigureDatabaseOptions(
        DbContextOptionsBuilder options,
        DatabaseType databaseType,
        string connectionString)
    {
        switch (databaseType)
        {
            case DatabaseType.SqlServer:
                options.UseSqlServer(connectionString);
                break;

            case DatabaseType.InMemory:
                options.UseInMemoryDatabase(connectionString);
                break;

            case DatabaseType.MySql:
                options.UseMySql(ServerVersion.AutoDetect(connectionString),
                    mysqlOptions => mysqlOptions.EnableRetryOnFailure(MaxRetryCount, TimeSpan.FromSeconds(MaxRetryDelay), null));
                break;

            case DatabaseType.Sqlite:
                options.UseSqlite(connectionString);
                break;

            case DatabaseType.PostgreSQL:
                options.UseNpgsql(connectionString);
                break;

            default:
                throw new ArgumentException("Invalid database type specified.", nameof(databaseType));
        }
    }

    private static async Task ApplyMigrations<TContext>(IServiceCollection services) where TContext : DbContext
    {
        using var serviceProvider = services.BuildServiceProvider();
        var logger = serviceProvider.GetService<ILogger>();
        var context = serviceProvider.GetService<TContext>();

        if (context == null)
        {
            logger?.LogError($"Unable to resolve {typeof(TContext).FullName}. Database migration will not be performed.");
            return;
        }

        try
        {
            await context.Database.MigrateAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "An error occurred while migrating the database.");
            throw;
        }
    }
}