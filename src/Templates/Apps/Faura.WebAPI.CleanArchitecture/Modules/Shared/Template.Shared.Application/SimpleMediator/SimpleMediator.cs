namespace Template.Shared.Application.SimpleMediator;

using System.Reflection;

/// <summary>
/// Simple implementation of the mediator pattern for CQRS.
/// Dispatches requests to appropriate handlers using dependency injection.
/// </summary>
/// <param name="serviceProvider">The service provider for resolving handlers.</param>
public class SimpleMediator(IServiceProvider serviceProvider): IMediator
{
    /// <summary>
    /// Sends a request to be handled by the appropriate handler.
    /// </summary>
    /// <typeparam name="TResponse">The type of response expected.</typeparam>
    /// <param name="request">The request to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response from the handler.</returns>
    public async Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var requestType = request.GetType();
        var responseType = typeof(TResponse);

        // Build the handler type: IRequestHandler<TRequest, TResponse>
        var handlerType = typeof(IRequestHandler<,>)
            .MakeGenericType(requestType, responseType);

        // Resolve the handler from the DI container
        var handler = serviceProvider.GetService(handlerType);

        if (handler == null)
        {
            throw new InvalidOperationException(
                $"No handler found for request type {requestType.Name}. " +
                $"Make sure to register IRequestHandler<{requestType.Name}, {responseType.Name}> in the DI container.");
        }

        // Invoke the Handle method
        var handleMethod = handlerType.GetMethod(nameof(IRequestHandler<IRequest<TResponse>, TResponse>.Handle));

        if (handleMethod == null)
        {
            throw new InvalidOperationException($"Handle method not found on handler for {requestType.Name}");
        }

        var result = handleMethod.Invoke(handler, new object[] { request, cancellationToken });

        if (result is Task<TResponse> task)
        {
            return await task;
        }

        throw new InvalidOperationException($"Handler for {requestType.Name} did not return a Task<{responseType.Name}>");
    }
}
