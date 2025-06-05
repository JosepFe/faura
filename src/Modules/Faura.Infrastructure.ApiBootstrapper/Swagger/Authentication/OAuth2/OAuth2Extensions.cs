using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Faura.Infrastructure.ApiBootstrapper.Swagger.Authentication.OAuth2;

public static class OAuth2Extensions
{
    public static SwaggerGenOptions AddOAuth2Authentication(
        this SwaggerGenOptions options,
        IConfiguration configuration
    )
    {
        var oauth2Options = configuration
            .GetSection(OAuth2Options.SectionName)
            .Get<OAuth2Options>();

        if (!oauth2Options?.Enable ?? true)
            return options;

        // Security Definition
        options.AddSecurityDefinition(
            oauth2Options.Name,
            new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(oauth2Options.AuthenticationURL),
                        Scopes = oauth2Options.Scopes,
                    },
                },
            }
        );

        // Security Requirement
        var requirement = new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = oauth2Options.Name,
                    },
                },
                oauth2Options.Scopes.Keys.ToList()
            },
        };

        options.AddSecurityRequirement(requirement);

        return options;
    }
}
