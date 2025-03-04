namespace Faura.Infrastructure.ApiBoostraper.Extensions;

using Faura.Infrastructure.ApiBoostraper.Middlewares;
using Microsoft.AspNetCore.Builder;

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
