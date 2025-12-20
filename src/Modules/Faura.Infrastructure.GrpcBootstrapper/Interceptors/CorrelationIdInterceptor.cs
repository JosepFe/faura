namespace Faura.Infrastructure.GrpcBootstrapper.Interceptors;

using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.HeaderPropagation;

public class CorrelationIdInterceptor : Interceptor
{
    private const string HeaderKey = "x-correlation-id";
    private readonly HeaderPropagationValues _headerPropagationValues;

    public CorrelationIdInterceptor(HeaderPropagationValues headerPropagationValues)
        => _headerPropagationValues = headerPropagationValues;

    public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var correlationId = context.RequestHeaders.GetValue(HeaderKey) ?? Guid.NewGuid().ToString();

        _headerPropagationValues.Headers?.TryAdd(HeaderKey, correlationId);

        context.ResponseTrailers.Add(HeaderKey, correlationId);

        return continuation(request, context);
    }
}
