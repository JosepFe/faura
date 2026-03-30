namespace Template.Api.Bootstrappers;

using Template.Shared.Infrastructure;

/// <summary>
/// Bootstrapper for shared services.
/// </summary>
public static class SharedBootstrapper
{
    /// <summary>
    /// Adds shared services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSharedServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSharedInfrastructure(configuration);

        // HttpClient Factory
        services.AddHttpClient();

        // Memory Cache
        services.AddMemoryCache();

        return services;
    }
}
