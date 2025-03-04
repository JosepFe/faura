namespace Faura.WebAPI.Boostrappers;

using Faura.Infrastructure.Logger;

public static class ApiBoostrapper
{
    public static WebApplicationBuilder RegisterDependencies(this WebApplicationBuilder builder)
    {
        builder.Host.SetupLogging();
        return builder;
    }
}