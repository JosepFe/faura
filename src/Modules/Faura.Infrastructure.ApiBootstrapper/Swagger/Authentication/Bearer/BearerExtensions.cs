namespace Faura.Infrastructure.ApiBootstrapper.Swagger.Authentication.Bearer;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class BearerExtensions
{
    public static SwaggerGenOptions AddBearerAuthentication(
        this SwaggerGenOptions options,
        IConfiguration configuration)
    {
        var bearerOptions = configuration
            .GetSection(BearerOptions.SectionName)
            .Get<BearerOptions>();

        if (!bearerOptions?.Enable ?? true)
            return options;

        // Security Definition
        options.AddSecurityDefinition(
            bearerOptions!.Name,
            new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme.",
            });

        // Security Requirement
        var requirement = new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = bearerOptions.Name,
                    },
                },
                new List<string>()
            },
        };

        options.AddSecurityRequirement(requirement);

        return options;
    }
}
