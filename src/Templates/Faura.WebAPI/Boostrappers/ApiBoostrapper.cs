namespace Faura.WebAPI.Boostrappers;

using Faura.Infrastructure.Logger;
using Faura.Infrastructure.JWT.Extensions;
using Faura.Infrastructure.ApiBoostraper.Extensions;

public static class ApiBoostrapper
{
    public static WebApplicationBuilder RegisterDependencies(this WebApplicationBuilder builder)
    {
        builder.RegisterSettingsProvider<Program>();
        builder.BootstrapCommonFauraServices();
        builder.Host.SetupLogging();
        builder.Services.SetUpJwt(builder.Configuration);

        return builder;
    }
}