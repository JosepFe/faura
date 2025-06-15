namespace Faura.Infrastructure.ApiBootstrapper.Extensions;

using Faura.Infrastructure.ApiBootstrapper.Swagger.Authentication.ApiKey;
using Faura.Infrastructure.ApiBootstrapper.Swagger.Authentication.BasicAuth;
using Faura.Infrastructure.ApiBootstrapper.Swagger.Authentication.Bearer;
using Faura.Infrastructure.ApiBootstrapper.Swagger.Authentication.OAuth2;
using Faura.Infrastructure.Common.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

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

    public static IServiceCollection SetUpSwagger(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        if (FauraEnvironment.IsProduction)
        {
            return services;
        }

        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));

            options.AddSwaggerSecurityConfigurations(configuration);
        });

        return services;
    }

    private static SwaggerGenOptions AddSwaggerSecurityConfigurations(
        this SwaggerGenOptions options,
        IConfiguration configuration)
        => options
            .AddOAuth2Authentication(configuration)
            .AddBearerAuthentication(configuration)
            .AddBasicAuthentication(configuration)
            .AddApiKeyAuthentication(configuration);
}
