﻿namespace Faura.Infrastructure.ApiBoostraper.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder BootstrapCommonFauraServices(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.SetUpSwagger(builder.Configuration);

        builder.Services.AddHeadersPropagation(builder.Configuration);
        return builder;
    }

    public static void RegisterSettingsProvider<T>(this IHostApplicationBuilder builder) where T : class
    {
        builder.Configuration.AddEnvironmentVariables();
        builder.Configuration.ConfigureUserSecrets<T>();
    }
}
