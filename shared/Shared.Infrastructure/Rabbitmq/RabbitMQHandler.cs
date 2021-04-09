using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Shared.Infrastructure
{
    public class RabbitMQHandler
    {
        ConnectionFactory connectionFactory;
        IConnection connection;
        IModel channel;
        string exchangeName;

        public RabbitMQHandler(string hostName, string userName, string password, string changeName="fanout_mq")
        {
            exchangeName = changeName;
            connectionFactory = new ConnectionFactory
            {
                HostName = hostName,
                UserName = userName,
                Password = password
            };
            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
        }

        public void SendMsg<T>(string queName, T msg) where T : class
        {
            channel.QueueDeclare(queName, true, false, false, null);
            channel.QueueBind(queName, exchangeName, queName);

            var basicProperties = channel.CreateBasicProperties();
            basicProperties.DeliveryMode = 2;
            var payload = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(msg));
            var address = new PublicationAddress(ExchangeType.Direct, exchangeName, queName);
            channel.BasicPublish(address, basicProperties, payload);
        }

        public void SendMsg(string queName, string msg)
        {
            channel.QueueDeclare(queName, true, false, false, null);
            channel.QueueBind(queName, exchangeName, queName);

            var basicProperties = channel.CreateBasicProperties();
            basicProperties.DeliveryMode = 2;
            var payload = Encoding.UTF8.GetBytes(msg);
            var address = new PublicationAddress(ExchangeType.Direct, exchangeName, queName);
            channel.BasicPublish(address, basicProperties, payload);
        }

        public void Receive(string queName, Action<string> received)
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            consumer.Received += (ch, ea) =>
            {
                string message = Encoding.UTF8.GetString(ea.Body.ToArray());
                received(message);
                channel.BasicAck(ea.DeliveryTag, false);
            };
            channel.BasicConsume(queName, false, consumer);
        }
    }
}
