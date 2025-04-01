namespace Faura.Infrastructure.Messaging.Services.Producer;

public interface IPulsarProducerMessageService
{
    Task<string> SendMessageAsync(string topicName, byte[] messageData, Dictionary<string, string>? metadata = null);
    Task<string> SendTypedMessageAsync<T>(string topicName, T message, Dictionary<string, string>? metadata = null);
}
