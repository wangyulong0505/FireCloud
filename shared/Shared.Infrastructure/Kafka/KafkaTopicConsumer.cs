using Confluent.Kafka;
using System;

namespace Shared.Infrastructure
{
    public class KafkaTopicConsumer<TKey, TValue>
    {
        public delegate void OnReceivedHandle(object data);
        public event OnReceivedHandle onReceivedHandle;

        private IConsumer<TKey, TValue> consumer;
        private string BootStrapServer;
        private string GroupId;

        public KafkaTopicConsumer(string bootStrapServer, string groupId)
        {
            BootStrapServer = bootStrapServer;
            GroupId = groupId;
        }

        public bool Init()
        {
            try
            {
                var conf = new ConsumerConfig
                {
                    GroupId = GroupId,
                    BootstrapServers = BootStrapServer,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = false
                };
                consumer = new ConsumerBuilder<TKey, TValue>(conf)
                    .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                    .Build();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void Subscribe(string topic, bool isCommit)
        {
            try
            {
                if(consumer != null)
                {
                    consumer.Subscribe(topic);
                    while(true)
                    {
                        var consume = consumer.Consume();
                        if(onReceivedHandle != null)
                        {
                            onReceivedHandle(consume);
                            if(isCommit)
                            {
                                consumer.Commit(consume);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void UnSubscribe()
        {
            if(consumer != null)
            {
                consumer.Unsubscribe();
            }
        }
    }
}
