namespace Faura.Grpc.Bootstrappers;

public static class ApplicationBootstrapper
{
    public static WebApplicationBuilder RegisterApplicationDependencies(
        this WebApplicationBuilder builder
    )
    {
        return builder;
    }
}
