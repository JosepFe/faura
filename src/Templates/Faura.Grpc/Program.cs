using Faura.Grpc.Boostrappers;
using Faura.Infrastructure.GrpcBootstrapper.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterDependencies();
builder.RegisterApplicationDependencies();

var app = builder.Build();

app.ConfigureCommonFauraWebApplication(GrpcBoostrapper.RegisterGrpcServices());

app.Run();
