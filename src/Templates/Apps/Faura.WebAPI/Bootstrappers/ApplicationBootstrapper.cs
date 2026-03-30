using Faura.WebAPI.Application;
using Faura.WebAPI.Infrastructure.Persistence;

namespace Faura.WebAPI.Bootstrappers;

/// <summary>
/// Bootstrapper for application layer dependencies.
/// </summary>
public static class ApplicationBootstrapper
{
    public static WebApplicationBuilder RegisterApplicationDependencies(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ISampleService, SampleService>();
        builder.Services.AddScoped<ISampleUoW, SampleUoW>();
        
        return builder;
    }
}
