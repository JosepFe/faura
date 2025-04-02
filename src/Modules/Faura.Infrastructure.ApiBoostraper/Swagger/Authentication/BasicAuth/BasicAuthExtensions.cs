namespace Faura.Infrastructure.ApiBoostraper.Swagger.Authentication.BasicAuth;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class BasicAuthExtensions
{
    public static SwaggerGenOptions AddBasicAuthentication(this SwaggerGenOptions options, IConfiguration configuration)
    {
        var basicAuthOptions = configuration.GetSection(BasicAuthOptions.SectionName).Get<BasicAuthOptions>();

        if (!basicAuthOptions?.Enable ?? true) return options;

        // Security Definition
        options.AddSecurityDefinition(basicAuthOptions.Name, new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "basic",
            Description = "Basic Authentication"
        });

        // Security Requirement
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = basicAuthOptions.Name
                    }
                },
                new List<string>()
            }
        });

        return options;
    }
}