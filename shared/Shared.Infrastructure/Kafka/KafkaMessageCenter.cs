using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Infrastructure.Kafka
{
    public static class KafkaMessageCenter<TKey, TValue>
    {
        public static IKafkaProducer<TKey, TValue> CreateTopicProducer(string bootstrapServer)
        {
            if (string.IsNullOrEmpty(bootstrapServer))
            {
                return null;
            }
            var producer = new KafkaTopicProducer<TKey, TValue>(bootstrapServer);
            if (!producer.Init())
            {
                return null;
            }
            return producer;
        }

        /// <summary>
        /// 创建消费者
        /// </summary>
        /// <param name="bootstrapServer"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static KafkaTopicConsumer<TKey, TValue> CreateTopicConsumer(string bootstrapServer, string groupId = "default-consumer-group")
        {
            if (string.IsNullOrEmpty(bootstrapServer))
            {
                return null;
            }
            var consumer = new KafkaTopicConsumer<TKey, TValue>(bootstrapServer, groupId);
            if (!consumer.Init())
            {
                return null;
            }
            return consumer;
        }
    }
}
