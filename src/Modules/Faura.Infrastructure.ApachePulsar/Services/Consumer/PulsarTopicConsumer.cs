namespace Faura.Infrastructure.ApachePulsar.Services.Consumer;

using DotPulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;

public abstract class PulsarTopicConsumer<T> : BackgroundService
{
    private readonly ILogger<PulsarTopicConsumer<T>> _logger;
    private readonly IPulsarClient _pulsarClient;
    private readonly string _topicName;
    private readonly string _subscriptionName;

    protected PulsarTopicConsumer(
        ILogger<PulsarTopicConsumer<T>> logger,
        IPulsarClient pulsarClient,
        string topicName,
        string subscriptionName)
    {
        _logger = logger;
        _pulsarClient = pulsarClient;
        _topicName = topicName;
        _subscriptionName = subscriptionName;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var consumer = _pulsarClient.NewConsumer()
            .SubscriptionName(_subscriptionName)
            .Topic(_topicName)
            .SubscriptionType(SubscriptionType.Shared)
            .Create();

        _logger.LogInformation("Initiating consumer for topic {Topic} with subscription {Subscription}",
            _topicName, _subscriptionName);

        await foreach (var message in consumer.Messages(stoppingToken))
        {
            try
            {
                var messageData = Encoding.UTF8.GetString(message.Data.FirstSpan);
                _logger.LogDebug("Message received: {Message}", messageData);

                var payload = ProcessMessage(messageData);

                if (payload != null)
                {
                    await HandleMessageAsync(payload, stoppingToken);
                }

                await consumer.Acknowledge(message, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing the message from topic {Topic}", _topicName);
            }
        }
    }

    protected abstract T ProcessMessage(string messageData);

    protected abstract Task HandleMessageAsync(T payload, CancellationToken cancellationToken);
}