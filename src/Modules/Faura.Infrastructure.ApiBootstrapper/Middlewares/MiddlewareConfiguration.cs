using Faura.Infrastructure.ApiBoostraper.Middlewares.CorrelationId;
using Microsoft.AspNetCore.Builder;

namespace Faura.Infrastructure.ApiBootstrapper.Middlewares;

public static class MiddlewareConfiguration
{
    public static void ConfigureMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
    }
}
