using Template.Api.Bootstrappers;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddApiServices()
    .Services
    .AddSharedServices(builder.Configuration)
    .AddModules(builder.Configuration);

var app = builder.Build();

app.ConfigureApiWebApplication();

await app.RunAsync();

/// <summary>
/// Program class for integration testing support.
/// </summary>
public partial class Program
{
}
