namespace Shared.Infrastructure
{
    public class MQHandlerFactory
    {
        public static RabbitMQHandler Default() => new RabbitMQHandler("", "", "exchange_fanout");
    }
}
