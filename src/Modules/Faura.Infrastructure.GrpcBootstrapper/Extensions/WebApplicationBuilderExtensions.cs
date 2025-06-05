using Faura.Infrastructure.GrpcBootstrapper.Interceptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Faura.Infrastructure.GrpcBootstrapper.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder BootstrapCommonFauraServices(
        this WebApplicationBuilder builder
    )
    {
        builder.Services.AddGrpc(options =>
        {
            options.Interceptors.Add<CorrelationIdInterceptor>();
        });

        builder.Services.AddHeadersPropagation(builder.Configuration);

        return builder;
    }

    public static void RegisterSettingsProvider<T>(this IHostApplicationBuilder builder)
        where T : class
    {
        builder.Configuration.AddEnvironmentVariables();
        builder.Configuration.ConfigureUserSecrets<T>();
    }
}
