using Microsoft.Extensions.DependencyInjection;

namespace Faura.Infrastructure.ApiBootstrapper.Extensions;

public static class ApiExtensions
{
    /// <summary>
    /// Configures controllers.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> services.</param>
    /// <returns><see cref="IMvcBuilder"/></returns>
    public static IMvcBuilder ConfigureControllers(this IServiceCollection services) =>
        services.AddControllers();
}
