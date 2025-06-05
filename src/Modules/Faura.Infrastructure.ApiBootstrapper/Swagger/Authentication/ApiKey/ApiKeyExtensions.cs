using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Faura.Infrastructure.ApiBootstrapper.Swagger.Authentication.ApiKey;

public static class ApiKeyExtensions
{
    public static SwaggerGenOptions AddApiKeyAuthentication(
        this SwaggerGenOptions options,
        IConfiguration configuration
    )
    {
        var apiKeyOptions = configuration
            .GetSection(ApiKeyOptions.SectionName)
            .Get<ApiKeyOptions>();

        if (!apiKeyOptions?.Enable ?? true)
            return options;

        options.AddSecurityDefinition(
            apiKeyOptions!.Name,
            new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                Name = apiKeyOptions.Name,
                In = apiKeyOptions.In,
                Description = "API Key Authentication",
            }
        );

        options.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = apiKeyOptions.Name,
                        },
                    },
                    new List<string>()
                },
            }
        );

        return options;
    }
}
