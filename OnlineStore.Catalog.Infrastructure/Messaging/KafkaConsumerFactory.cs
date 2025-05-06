using Confluent.Kafka;
using Microsoft.Extensions.Options;
using OnlineStore.Catalog.Application.Abstractions;
using OnlineStore.Catalog.Contracts.Options;

namespace OnlineStore.Catalog.Infrastructure.Messaging
{
    public class KafkaConsumerFactory : IKafkaConsumerFactory
    {
        private readonly ConsumerConfig _config;

        public KafkaConsumerFactory(IOptions<KafkaOptions> kafkaOptions)
        {
            _config = new ConsumerConfig 
            {
                BootstrapServers = kafkaOptions.Value.BootstrapServers,
                GroupId = kafkaOptions.Value.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }

        public IConsumer<TKey, TValue> CreateConsumer<TKey, TValue>()
        {
            return new ConsumerBuilder<TKey, TValue>(_config).Build();
        }
    }
}
