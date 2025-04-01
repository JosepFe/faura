namespace Faura.Infrastructure.Messaging.Services;

public interface IPulsarMessagingService : IAsyncDisposable
{
    Task<T> ReceiveTypedMessageAsync<T>(string subscriptionName, CancellationToken cancellationToken = default);
    Task<IDisposable> SubscribeAsync<T>(string subscriptionName, Func<T, Task> messageHandler, CancellationToken cancellationToken = default);
    Task<IDisposable> SubscribeWithRetryAsync<T>(string subscriptionName, Func<T, Task> messageHandler, CancellationToken cancellationToken = default);
    Task<bool> ValidateSchemaAsync<T>(T message);
    Task<Dictionary<string, object>> GetMetricsAsync();
} 