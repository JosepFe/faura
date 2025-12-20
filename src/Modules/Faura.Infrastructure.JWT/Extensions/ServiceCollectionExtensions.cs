namespace Faura.Infrastructure.JWT.Extensions;

using Faura.Infrastructure.Common.Utils;
using Faura.Infrastructure.JWT.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection SetUpJwt(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtOptions = services.GetTypedOptions<JwtOptions>(
            configuration,
            JwtOptions.SectionName);

        services.AddAuthorization();
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.Audience = jwtOptions!.Audience!;
                options.MetadataAddress = jwtOptions.MetadataAddress!;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtOptions.ValidIssuer,
                };
            });

        return services;
    }
}
