using Faura.Infrastructure.ApiBoostraper.Extensions;
using Faura.WebAPI.Boostrappers;

var builder = WebApplication.CreateBuilder(args);

builder.BootstrapCommonFauraServices();
builder.RegisterDependencies();

var app = builder.Build();

app.ConfigureCommonFauraWebApplication();

app.Run();
