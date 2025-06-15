using Faura.Infrastructure.ApiBootstrapper.Extensions;
using Faura.WebAPI.Bootstrappers;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterDependencies();
builder.RegisterApplicationDependencies();

var app = builder.Build();

app.ConfigureCommonFauraWebApplication();

await app.RunAsync();

public partial class Program
{
    protected Program()
    {
    }
}
