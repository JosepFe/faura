namespace Faura.Infrastructure.GrpcBootstrapper.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class HeadersPropagationExtensions
{
    public const string CorrelationIdHeaderKey = "x-correlation-id";

    public static void AddHeadersPropagation(
        this WebApplicationBuilder builder) => builder.Services.AddHeadersPropagation();

    public static IServiceCollection AddHeadersPropagation(
        this IServiceCollection services)
    {
        services.AddHeaderPropagation(options => options.Headers.Add(CorrelationIdHeaderKey));
        return services;
    }
}
