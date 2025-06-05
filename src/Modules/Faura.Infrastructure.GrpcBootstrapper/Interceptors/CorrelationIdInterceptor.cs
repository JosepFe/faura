using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.HeaderPropagation;

namespace Faura.Infrastructure.GrpcBootstrapper.Interceptors;

public class CorrelationIdInterceptor : Interceptor
{
    private readonly HeaderPropagationValues _headerPropagationValues;
    private const string HeaderKey = "x-correlation-id";

    public CorrelationIdInterceptor(HeaderPropagationValues headerPropagationValues)
    {
        _headerPropagationValues = headerPropagationValues;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation
    )
    {
        var correlationId = context.RequestHeaders.GetValue(HeaderKey) ?? Guid.NewGuid().ToString();

        _headerPropagationValues.Headers?.TryAdd(HeaderKey, correlationId);

        context.ResponseTrailers.Add(HeaderKey, correlationId);

        return await continuation(request, context);
    }
}
