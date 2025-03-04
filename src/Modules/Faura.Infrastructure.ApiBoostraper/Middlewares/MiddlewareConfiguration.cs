namespace Faura.Infrastructure.ApiBoostraper.Middlewares
{
    using Microsoft.AspNetCore.Builder;
    using Faura.Infrastructure.ApiBoostraper.Middlewares.CorrelationId;

    public static class MiddlewareConfiguration
    {
        public static void ConfigureMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<CorrelationIdMiddleware>();
        }
    }
}
