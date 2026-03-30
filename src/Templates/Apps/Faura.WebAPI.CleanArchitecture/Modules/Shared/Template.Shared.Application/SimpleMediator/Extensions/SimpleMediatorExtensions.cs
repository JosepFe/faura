namespace Template.Shared.Application.SimpleMediator.Extensions;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for registering SimpleMediator in the DI container.
/// </summary>
public static class SimpleMediatorExtensions
{
    /// <summary>
    /// Adds SimpleMediator to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSimpleMediator(this IServiceCollection services)
    {
        services.AddScoped<IMediator, SimpleMediator>();
        return services;
    }
}
