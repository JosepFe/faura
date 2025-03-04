namespace Faura.Infrastructure.ApiBoostraper.Extensions;

using Faura.Configurations;
using Microsoft.AspNetCore.Builder;

public static class SwaggerExtensions
{
    /// <summary>
    /// Adds Swagger and Swagger UI configuration to the application.
    /// </summary>
    /// <param name="app"><see cref="IApplicationBuilder"/> application builder.</param>
    public static void ConfigureUseSwagger(this IApplicationBuilder app)
    {
        if (FauraEnvironment.IsProduction)
        {
            return;
        }

        app.UseSwagger();
        app.UseSwaggerUI();
    }
}
