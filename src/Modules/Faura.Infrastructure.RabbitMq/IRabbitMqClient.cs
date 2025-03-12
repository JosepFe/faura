namespace Faura.Infrastructure.RabbitMq
{
    public interface IRabbitMqClient
    {
        void SendMessage(string queueName, string message);
        void ReceiveMessages(string queueName);
        void Close();
    }
}
