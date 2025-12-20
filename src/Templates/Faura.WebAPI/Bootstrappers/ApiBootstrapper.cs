namespace Faura.WebAPI.Bootstrappers;

using Faura.Infrastructure.JWT.Extensions;
using Faura.Infrastructure.Logger;
using Faura.Infrastructure.ApiBootstrapper.Extensions;

public static class ApiBootstrapper
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
