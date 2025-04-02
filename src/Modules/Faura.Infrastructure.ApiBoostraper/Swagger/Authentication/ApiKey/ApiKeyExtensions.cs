namespace Faura.Infrastructure.ApiBoostraper.Swagger.Authentication.ApiKey;

using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class ApiKeyExtensions
{
    public static SwaggerGenOptions AddApiKeyAuthentication(this SwaggerGenOptions options, IConfiguration configuration)
    {
        var apiKeyOptions = configuration.GetSection(ApiKeyOptions.SectionName).Get<ApiKeyOptions>();

        if (!apiKeyOptions?.Enable ?? true) return options;

        options.AddSecurityDefinition(apiKeyOptions!.Name, new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.ApiKey,
            Name = apiKeyOptions.Name,
            In = apiKeyOptions.In,
            Description = "API Key Authentication"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = apiKeyOptions.Name
                    }
                },
                new List<string>()
            }
        });

        return options;
    }
}