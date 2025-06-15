using Faura.Grpc.Bootstrappers;
using Faura.Infrastructure.GrpcBootstrapper.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterDependencies();
builder.RegisterApplicationDependencies();

var app = builder.Build();

app.ConfigureCommonFauraWebApplication(GrpcBootstrapper.RegisterGrpcServices());

await app.RunAsync();

public partial class Program
{
    protected Program()
    {
    }
}
