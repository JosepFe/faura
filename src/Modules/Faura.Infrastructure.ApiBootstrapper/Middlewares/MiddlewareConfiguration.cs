namespace Faura.Infrastructure.ApiBootstrapper.Middlewares;

using Faura.Infrastructure.ApiBootstrapper.Middlewares.CorrelationId;
using Microsoft.AspNetCore.Builder;

public static class MiddlewareConfiguration
{
    public static void ConfigureMiddlewares(this IApplicationBuilder app)
        => app.UseMiddleware<CorrelationIdMiddleware>();
}
