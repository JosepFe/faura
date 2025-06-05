namespace Faura.Grpc.Bootstrappers;

public static class ApplicationBoostrapper
{
    public static WebApplicationBuilder RegisterApplicationDependencies(
        this WebApplicationBuilder builder
    )
    {
        return builder;
    }
}
