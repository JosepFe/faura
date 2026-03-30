namespace Template.Sample.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Sample.Application;
using Template.Sample.Domain.Repositories;
using Template.Sample.Infrastructure.Persistence;
using Template.Sample.Infrastructure.Persistence.Options;
using Template.Sample.Infrastructure.Repositories;

/// <summary>
/// Dependency injection configuration for Sample infrastructure.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds the Sample module to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSampleModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.SetupDatabase(configuration);
        services.AddRepositories();
        services.AddHandlers();

        return services;
    }

    private static IServiceCollection SetupDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration
            .GetSection(SampleMongoDbOptions.SectionName)
            .Get<SampleMongoDbOptions>()
            ?? throw new InvalidOperationException("Sample MongoDB settings not found");

        services.AddDbContext<SampleDbContext>(options =>
        {
            options.UseMongoDB(settings.MongoDb.ConnectionString, settings.MongoDb.DatabaseName);
        });

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISampleItemRepository, SampleItemRepository>();

        return services;
    }
}
