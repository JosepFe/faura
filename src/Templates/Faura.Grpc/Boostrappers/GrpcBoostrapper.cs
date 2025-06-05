namespace Faura.Grpc.Boostrappers;

using Faura.Grpc.Services;
using Faura.Infrastructure.GrpcBootstrapper.Extensions;
using Faura.Infrastructure.Logger;

public static class GrpcBoostrapper
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