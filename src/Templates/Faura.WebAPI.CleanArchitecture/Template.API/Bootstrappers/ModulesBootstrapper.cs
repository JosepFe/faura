namespace Template.Api.Bootstrappers;

using Template.Sample.Infrastructure;

/// <summary>
/// Bootstrapper for registering all modules.
/// </summary>
public static class ModulesBootstrapper
{
    /// <summary>
    /// Adds all modules to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddModules(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register modules here
        services.AddSampleModule(configuration);

        return services;
    }
}
