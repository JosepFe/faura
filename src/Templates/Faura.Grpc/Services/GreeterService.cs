using Faura.Infrastructure.Logger.Extensions;
using Grpc.Core;

namespace Faura.Grpc.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;

    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        _logger.LogFauraInformation("hola mundo");
        return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
    }
}
