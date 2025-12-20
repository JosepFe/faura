namespace Faura.Infrastructure.GrpcBootstrapper.Extensions;

using Faura.Infrastructure.GrpcBootstrapper.Interceptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder BootstrapCommonFauraServices(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddGrpc(options => options.Interceptors.Add<CorrelationIdInterceptor>());

        builder.Services.AddHeadersPropagation();

        return builder;
    }

    public static void RegisterSettingsProvider<T>(this IHostApplicationBuilder builder)
        where T : class
    {
        builder.Configuration.AddEnvironmentVariables();
        builder.Configuration.ConfigureUserSecrets<T>();
    }
}
