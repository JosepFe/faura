using Faura.Infrastructure.ApiBootstrapper.Extensions;
using Faura.WebAPI.Bootstrappers;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterDependencies();
builder.RegisterApplicationDependencies();
builder.Services.RegisterInfrastructureDependencies(builder.Configuration);
builder.Services.RegisterOptions(builder.Configuration);

var app = builder.Build();

app.ConfigureCommonFauraWebApplication();
await app.ConfigureInfrastructureAsync(app.Environment);

await app.RunAsync();

// This is needed for the integration tests
#pragma warning disable S1118 // Utility classes should not have public constructors,
public partial class Program
#pragma warning restore S1118 // Utility classes should not have public constructors
{
}
