using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Infrastructure.Rabbitmq
{
    public class MQHandlerFactory
    {
        public static RabbitMQHandler Default() => new RabbitMQHandler("", "", "exchange_fanout");
    }
}
