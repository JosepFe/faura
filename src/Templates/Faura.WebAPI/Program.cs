using Faura.Infrastructure.ApiBoostraper.Extensions;
using Faura.WebAPI.Boostrappers;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterDependencies();
builder.RegisterApplicationDependencies();

var app = builder.Build();

app.ConfigureCommonFauraWebApplication();

app.Run();
