namespace Faura.Infrastructure.ApiBootstrapper.Extensions;

using Microsoft.Extensions.DependencyInjection;

public static class ApiExtensions
{
    /// <summary>
    /// Configures controllers.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> services.</param>
    public static IMvcBuilder ConfigureControllers(this IServiceCollection services) =>
        services.AddControllers();
}
