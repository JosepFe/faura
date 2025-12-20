using Faura.WebAPI.Application;
using Faura.WebAPI.Infrastructure.Persistence;

namespace Faura.WebAPI.Bootstrappers;

public static class ApplicationBootstrapper
{
    public static WebApplicationBuilder RegisterApplicationDependencies(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<IEmployeeUoW, EmployeeUoW>();
        return builder;
    }
}
