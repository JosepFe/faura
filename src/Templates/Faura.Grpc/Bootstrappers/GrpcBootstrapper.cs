using Faura.Grpc.Services;
using Faura.Infrastructure.GrpcBootstrapper.Extensions;
using Faura.Infrastructure.Logger;

namespace Faura.Grpc.Bootstrappers;

public static class GrpcBootstrapper
{
    public static WebApplicationBuilder RegisterDependencies(this WebApplicationBuilder builder)
    {
        builder.RegisterSettingsProvider<Program>();
        builder.BootstrapCommonFauraServices();
        builder.Host.SetupLogging();

        return builder;
    }

    public static Action<IEndpointRouteBuilder> RegisterGrpcServices()
    {
        return endpoints =>
        {
            endpoints.MapGrpcService<GreeterService>();
        };
    }
}
