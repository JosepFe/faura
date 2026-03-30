using Faura.Infrastructure.Logger.Extensions;
using Grpc.Core;

namespace Faura.Grpc.Services;

/// <summary>
/// Greeter service implementation.
/// Uses C# 12 primary constructor.
/// </summary>
public class GreeterService(ILogger<GreeterService> logger) : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        logger.LogFauraInformation("hola mundo");
        return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
    }
}
