using Confluent.Kafka;
using System;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Kafka
{
    internal class KafkaTopicProducer<TKey, TValue> : IKafkaProducer<TKey, TValue>
    {
        private IProducer<TKey, TValue> producer;
        private string _bootStrapServer;
        public KafkaTopicProducer(string bootStrapServer)
        {
            _bootStrapServer = bootStrapServer;
        }
        public bool Init()
        {
            try
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = _bootStrapServer
                };
                producer = new ProducerBuilder<TKey, TValue>(config)
                    .SetErrorHandler((producer, error) =>
                    {
                        Console.WriteLine($"Kafka Error Handler {_bootStrapServer}, ErrorCode: {error.Code}, Reason: {error.Reason}");
                    })
                    .SetLogHandler((producer, msg) =>
                    {
                        Console.WriteLine($"Kafka Log Handler {_bootStrapServer}, Name: {msg.Name}, Message: {msg.Message}");
                    })
                    .Build();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public ISendResult Send(TKey key, TValue value, string topic, Action<DeliveryReport<TKey, TValue>> sendCallBack = null)
        {
            try
            {
                if(producer != null)
                {
                    var message = new Message<TKey, TValue>
                    {
                        Value = value,
                        Key = key
                    };
                    producer.Produce(topic, message, sendCallBack);
                    return new SendResult(true);
                }
                else
                {
                    return new SendResult(true);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public ISendResult Send(TKey key, TValue value, TopicPartition topicPartition, Action<DeliveryReport<TKey, TValue>> sendCallBack = null)
        {
            try
            {
                if (producer != null)
                {
                    var message = new Message<TKey, TValue>
                    {
                        Value = value,
                        Key = key
                    };
                    producer.Produce(topicPartition, message, sendCallBack);
                    return new SendResult(true);
                }
                else
                {
                    return new SendResult(true, "没有初始化生产者");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ISendResult SendAsync(TKey key, TValue value, string topic)
        {
            try
            {
                if (producer != null)
                {
                    var message = new Message<TKey, TValue>
                    {
                        Value = value,
                        Key = key
                    };
                    var deliveryReport = producer.ProduceAsync(topic, message);
                    deliveryReport.ContinueWith(task =>
                    {
                        Console.WriteLine("Producer: " + producer.Name + "\r\nTopic: " + topic + "\r\nPartition: " + task.Result.Partition + "\r\nOffset: " + task.Result.Offset);
                    });
                    producer.Flush(TimeSpan.FromSeconds(10));
                    return new SendResult(true);
                }
                else
                {
                    return new SendResult(true, "没有初始化生产者");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ISendResult SendAsync(TKey key, TValue value, TopicPartition topicPartition)
        {
            try
            {
                if (producer != null)
                {
                    var message = new Message<TKey, TValue>
                    {
                        Value = value,
                        Key = key
                    };

                    var deliveryReport = producer.ProduceAsync(topicPartition, message);
                    deliveryReport.ContinueWith(task =>
                    {
                        Console.WriteLine("Producer: " + producer.Name + "\r\nTopic: " + topicPartition.Topic + "\r\nPartition: " + task.Result.Partition + "\r\nOffset: " + task.Result.Offset);
                    });

                    producer.Flush(TimeSpan.FromSeconds(10));
                    return new SendResult(true);
                }
                else
                {
                    return new SendResult(true, "没有初始化生产者");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
