using Confluent.Kafka;
using System;

namespace Shared.Infrastructure
{
    public interface IKafkaProducer<TKey, TValue>
    {
        ISendResult Send(TKey key, TValue value, string topic, Action<DeliveryReport<TKey, TValue>> sendCallBack = null);
        ISendResult Send(TKey key, TValue value, TopicPartition topicPartition, Action<DeliveryReport<TKey, TValue>> sendCallBack = null);

        ISendResult SendAsync(TKey key, TValue value, string topic);
        ISendResult SendAsync(TKey key, TValue value, TopicPartition topicPartition);
    }
}
