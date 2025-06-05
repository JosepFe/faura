using Faura.Grpc.Boostrappers;
using Faura.Grpc.Services;
using Faura.Infrastructure.GrpcBootstrapper.Extensions;
using Faura.Grpc.Boostrappers;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterDependencies();
builder.RegisterApplicationDependencies();

var app = builder.Build();

app.ConfigureCommonFauraWebApplication(GrpcBoostrapper.RegisterGrpcServices());

app.Run();
