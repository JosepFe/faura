namespace Faura.Infrastructure.GrpcBootstrapper.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

public static class WebApplicationExtensions
{
    public static IApplicationBuilder ConfigureCommonFauraWebApplication(
        this IApplicationBuilder app,
        Action<IEndpointRouteBuilder> mapGrpcServices)
    {
        app.UseRouting();

        app.UseHeaderPropagation();

        app.UseEndpoints(endpoints => mapGrpcServices(endpoints));

        return app;
    }
}
