using Faura.Infrastructure.ApiBootstrapper.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Faura.Infrastructure.ApiBootstrapper.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureCommonFauraWebApplication(this WebApplication app)
    {
        app.ConfigureUseSwagger();

        app.UseHeaderPropagation();
        app.ConfigureMiddlewares();

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
