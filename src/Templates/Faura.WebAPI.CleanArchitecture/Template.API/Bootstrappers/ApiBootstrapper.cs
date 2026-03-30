namespace Template.Api.Bootstrappers;

using Faura.Infrastructure.ApiBoostraper.Extensions;
using Faura.Infrastructure.Logger;

/// <summary>
/// Bootstrapper for API-specific services.
/// </summary>
public static class ApiBootstrapper
{
    /// <summary>
    /// Adds API services to the application builder.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The web application builder for chaining.</returns>
    public static WebApplicationBuilder AddApiServices(this WebApplicationBuilder builder)
    {
        builder.RegisterSettingsProvider<Program>();
        builder.BootstrapCommonFauraServices();
        builder.Host.SetupLogging();

        // CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        builder.Services.AddHealthChecks();

        return builder;
    }

    /// <summary>
    /// Configures the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The web application for chaining.</returns>
    public static WebApplication ConfigureApiWebApplication(this WebApplication app)
    {
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();

        app.ConfigureCommonFauraWebApplication();

        app.MapHealthChecks("/health");

        return app;
    }
}
