namespace Faura.Infrastructure.Messaging.Services.Subscriptions;

using System.Buffers;
using System.Text;
using System.Text.Json;
using DotPulsar;
using DotPulsar.Abstractions;

internal class Subscription<T> : IDisposable
{
    private readonly IConsumer<ReadOnlySequence<byte>> _consumer;
    private readonly Func<T, Task> _messageHandler;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly Task _processingTask;

    public Subscription(IConsumer<ReadOnlySequence<byte>> consumer, Func<T, Task> messageHandler, CancellationToken cancellationToken)
    {
        _consumer = consumer;
        _messageHandler = messageHandler;
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _processingTask = ProcessMessagesAsync();
    }

    public async Task StartAsync()
    {
        await _processingTask;
    }

    private async Task ProcessMessagesAsync()
    {
        try
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                var message = await _consumer.Receive(_cancellationTokenSource.Token);
                var jsonMessage = Encoding.UTF8.GetString(message.Data.ToArray());
                var typedMessage = JsonSerializer.Deserialize<T>(jsonMessage);

                await _messageHandler(typedMessage);
                await _consumer.Acknowledge(message.MessageId);
            }
        }
        catch (OperationCanceledException)
        {
            // Normal cancellation, do nothing
        }
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _processingTask.Wait();
        _cancellationTokenSource.Dispose();
    }
} 