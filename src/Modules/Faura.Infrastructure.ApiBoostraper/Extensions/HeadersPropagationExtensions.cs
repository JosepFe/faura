namespace Faura.Infrastructure.ApiBoostraper.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class HeadersPropagationExtensions
{
    public const string CorrelationIdHeaderKey = "x-correlation-id";

    public static void AddHeadersPropagation(this WebApplicationBuilder builder, IConfiguration config)
        => builder.Services.AddHeadersPropagation(config);

    public static IServiceCollection AddHeadersPropagation(this IServiceCollection services, IConfiguration config)
    {
        services.AddHeaderPropagation(options =>
        {
            options.Headers.Add(CorrelationIdHeaderKey);
        });
        return services;
    }
}
