namespace Faura.Infrastructure.Messaging.Services;

using System.Buffers;
using System.Text;
using System.Text.Json;
using DotPulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;
using Faura.Infrastructure.Messaging.Options;
using Faura.Infrastructure.Messaging.Services.Subscriptions;
using Microsoft.Extensions.Options;

public class PulsarMessagingService : IPulsarMessagingService
{
    private readonly IPulsarClient _client;
    private readonly PulsarOptions _options;
    private readonly IProducer<ReadOnlySequence<byte>> _producer;
    private readonly Dictionary<string, IConsumer<ReadOnlySequence<byte>>> _consumers;
    private readonly Dictionary<string, object> _metrics;

    public PulsarMessagingService(IOptions<PulsarOptions> options)
    {
        _options = options.Value;
        _client = PulsarClient.Builder()
            .ServiceUrl(new Uri(_options.ServiceUrl))
            .Build();
        
        _producer = _client.NewProducer()
            .Topic(_options.Topic)
            .Create();

        _consumers = new Dictionary<string, IConsumer<ReadOnlySequence<byte>>>();
        _metrics = new Dictionary<string, object>();
    }

    public async Task<string> SendMessageAsync(byte[] messageData, Dictionary<string, string>? metadata = null)
    {
        var messageMetadata = new MessageMetadata();
        if (metadata != null)
        {
            foreach (var kvp in metadata)
            {
                messageMetadata[kvp.Key] = kvp.Value;
            }
        }

        var sequence = new ReadOnlySequence<byte>(messageData);
        var messageId = await _producer.Send(sequence);
        return messageId.ToString();
    }

    public async Task<string> SendTypedMessageAsync<T>(T message, Dictionary<string, string>? metadata = null)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        var messageData = Encoding.UTF8.GetBytes(jsonMessage);
        return await SendMessageAsync(messageData, metadata);
    }

    public async Task<T> ReceiveTypedMessageAsync<T>(string subscriptionName, CancellationToken cancellationToken = default)
    {
        var consumer = GetOrCreateConsumer(subscriptionName);
        var message = await consumer.Receive(cancellationToken);
        var jsonMessage = Encoding.UTF8.GetString(message.Data.ToArray());
        var typedMessage = JsonSerializer.Deserialize<T>(jsonMessage);

        await consumer.Acknowledge(message);

        return typedMessage;
    }

    public async Task<IDisposable> SubscribeAsync<T>(string subscriptionName, Func<T, Task> messageHandler, CancellationToken cancellationToken = default)
    {
        var consumer = GetOrCreateConsumer(subscriptionName);
        var subscription = new Subscription<T>(consumer, messageHandler, cancellationToken);
        await subscription.StartAsync();
        return subscription;
    }

    public async Task<IDisposable> SubscribeWithRetryAsync<T>(string subscriptionName, Func<T, Task> messageHandler, CancellationToken cancellationToken = default)
    {
        var consumer = GetOrCreateConsumer(subscriptionName);
        var subscription = new RetryableSubscription<T>(consumer, messageHandler, _options.RetryCount, _options.RetryInterval, cancellationToken);
        await subscription.StartAsync();
        return subscription;
    }

    public async Task<bool> ValidateSchemaAsync<T>(T message)
    {
        if (!_options.EnableSchemaValidation)
            return true;

        // Implementar validación de esquema según el tipo configurado
        return true;
    }

    public Task<Dictionary<string, object>> GetMetricsAsync()
    {
        return Task.FromResult(_metrics);
    }

    private IConsumer<ReadOnlySequence<byte>> GetOrCreateConsumer(string subscriptionName)
    {
        if (!_consumers.TryGetValue(subscriptionName, out var consumer))
        {
            consumer = _client.NewConsumer()
                .Topic(_options.Topic)
                .SubscriptionName(subscriptionName)
                .Create();

            _consumers[subscriptionName] = consumer;
        }

        return consumer;
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var consumer in _consumers.Values)
        {
            await consumer.DisposeAsync();
        }

        await _producer.DisposeAsync();
        await _client.DisposeAsync();
    }
} 