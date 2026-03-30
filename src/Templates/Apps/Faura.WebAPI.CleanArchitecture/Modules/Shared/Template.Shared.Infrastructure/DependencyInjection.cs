namespace Template.Shared.Infrastructure;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Shared.Application.SimpleMediator.Extensions;
using Template.Shared.Domain.Repositories;
using Template.Shared.Infrastructure.Repositories;

/// <summary>
/// Dependency injection configuration for shared infrastructure.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds shared infrastructure services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IEntityRepository<>), typeof(EntityRepository<>));
        services.AddSimpleMediator();

        return services;
    }
}
