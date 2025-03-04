namespace Faura.Infrastructure.ApiBoostraper.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class WebApplicationBuilderExtensions
{

    public static WebApplicationBuilder BootstrapCommonFauraServices(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddHeadersPropagation(builder.Configuration);
        return builder;
    }
}
