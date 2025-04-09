using Faura.Infrastructure.ApiBoostraper.Extensions;
using Faura.WebAPI.Boostrappers;

var builder = WebApplication.CreateBuilder(args);

var a = builder.Configuration.Sources;

builder.RegisterDependencies();
builder.RegisterApplicationDependencies();

var app = builder.Build();

app.ConfigureCommonFauraWebApplication();

app.Run();
