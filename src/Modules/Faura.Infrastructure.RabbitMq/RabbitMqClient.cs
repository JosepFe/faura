using RabbitMQ.Client;
using System.Text;

namespace Faura.Infrastructure.RabbitMq
{
    public class RabbitMqClient
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqClient(string hostName)
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void SendMessage(string queueName, string message)
        {
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }

        public void ReceiveMessages(string queueName)
        {
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Received {0}", message);
            };
            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        public void Close()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
