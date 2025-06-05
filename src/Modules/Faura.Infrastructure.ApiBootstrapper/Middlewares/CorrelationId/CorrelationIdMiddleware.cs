using Faura.Infrastructure.ApiBootstrapper.Extensions;
using Microsoft.AspNetCore.HeaderPropagation;
using Microsoft.AspNetCore.Http;

namespace Faura.Infrastructure.ApiBoostraper.Middlewares.CorrelationId;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HeaderPropagationValues _headerPropagationValues;

    public CorrelationIdMiddleware(
        RequestDelegate next,
        HeaderPropagationValues headerPropagationValues
    )
    {
        _next = next;
        _headerPropagationValues = headerPropagationValues;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context
            .Request.Headers[HeadersPropagationExtensions.CorrelationIdHeaderKey]
            .ToString();
        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
            context.Request.Headers.TryAdd(
                HeadersPropagationExtensions.CorrelationIdHeaderKey,
                correlationId
            );
            _headerPropagationValues.Headers!.TryAdd(
                HeadersPropagationExtensions.CorrelationIdHeaderKey,
                correlationId
            );
        }

        var responseHeader = context
            .Response.Headers[HeadersPropagationExtensions.CorrelationIdHeaderKey]
            .ToString();
        if (string.IsNullOrWhiteSpace(responseHeader))
        {
            context.Response.Headers.TryAdd(
                HeadersPropagationExtensions.CorrelationIdHeaderKey,
                correlationId
            );
        }

        await _next(context);
    }
}
