namespace Faura.Grpc.Boostrappers;

public static class ApplicationBoostrapper
{
    public static WebApplicationBuilder RegisterApplicationDependencies(this WebApplicationBuilder builder)
    {
        return builder;
    }
}