using Confluent.Kafka;

namespace OnlineStore.Catalog.Application.Abstractions
{
    public interface IKafkaConsumerFactory
    {
        IConsumer<TKey, TValue> CreateConsumer<TKey, TValue>();
        //IConsumer<TKey, TValue> CreateConsumerWithConfig<TKey, TValue>(Action<ConsumerConfig> configure);
    }
}
