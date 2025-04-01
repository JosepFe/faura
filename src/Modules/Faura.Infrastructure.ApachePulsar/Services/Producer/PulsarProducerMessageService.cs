namespace Faura.Infrastructure.Messaging.Services.Producer;

using DotPulsar.Abstractions;
using DotPulsar;
using Faura.Infrastructure.Messaging.Options;
using Microsoft.Extensions.Options;
using System.Buffers;
using System.Text.Json;
using System.Text;
using DotPulsar.Extensions;

public class PulsarProducerMessageService : IPulsarProducerMessageService
{
    private readonly IPulsarClient _client;
    private readonly IOptions<PulsarOptions> _pulsarOptions;

    public PulsarProducerMessageService(IOptions<PulsarOptions> options, IPulsarClient client)
    {
        _pulsarOptions = options;
        _client = client;
    }

    public async Task<string> SendMessageAsync(string topicName, byte[] messageData, Dictionary<string, string>? metadata = null)
    {
        var messageMetadata = new MessageMetadata();
        if (metadata != null)
        {
            foreach (var kvp in metadata)
            {
                messageMetadata[kvp.Key] = kvp.Value;
            }
        }

        var producer = CreatePulsarProducer(_client, topicName);

        var sequence = new ReadOnlySequence<byte>(messageData);
        var messageId = await producer.Send(sequence);
        return messageId.ToString();
    }

    public async Task<string> SendTypedMessageAsync<T>(string topicName, T message, Dictionary<string, string>? metadata = null)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        var messageData = Encoding.UTF8.GetBytes(jsonMessage);
        return await SendMessageAsync(topicName, messageData, metadata);
    }

    private IProducer<ReadOnlySequence<byte>> CreatePulsarProducer(IPulsarClient client, string topic)
    {
        return client.NewProducer()
            .Topic(topic)
            .ProducerName(_pulsarOptions.Value.ProducerName ?? string.Empty)
            .Create();
    }
}
