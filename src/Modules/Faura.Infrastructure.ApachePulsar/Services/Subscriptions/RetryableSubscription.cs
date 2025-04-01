namespace Faura.Infrastructure.Messaging.Services.Subscriptions;

using System.Buffers;
using System.Text;
using System.Text.Json;
using DotPulsar.Abstractions;

internal class RetryableSubscription<T> : IDisposable
{
    private readonly IConsumer<ReadOnlySequence<byte>> _consumer;
    private readonly Func<T, Task> _messageHandler;
    private readonly int _maxRetries;
    private readonly TimeSpan _retryInterval;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly Task _processingTask;

    public RetryableSubscription(IConsumer<ReadOnlySequence<byte>> consumer, Func<T, Task> messageHandler, int maxRetries, TimeSpan retryInterval, CancellationToken cancellationToken)
    {
        _consumer = consumer;
        _messageHandler = messageHandler;
        _maxRetries = maxRetries;
        _retryInterval = retryInterval;
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

                var retryCount = 0;
                while (retryCount < _maxRetries)
                {
                    try
                    {
                        await _messageHandler(typedMessage);
                        //await _consumer.Acknowledge(message);
                        break;
                    }
                    catch (Exception)
                    {
                        retryCount++;
                        if (retryCount >= _maxRetries)
                        {
                            await _consumer.RedeliverUnacknowledgedMessages();
                            throw;
                        }
                        await Task.Delay(_retryInterval, _cancellationTokenSource.Token);
                    }
                }
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