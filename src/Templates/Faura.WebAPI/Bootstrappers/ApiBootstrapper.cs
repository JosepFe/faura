using Faura.Infrastructure.ApiBootstrapper.Extensions;
using Faura.Infrastructure.JWT.Extensions;
using Faura.Infrastructure.Logger;

namespace Faura.WebAPI.Bootstrappers;

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
