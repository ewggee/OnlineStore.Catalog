using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnlineStore.Catalog.Application.Abstractions;
using OnlineStore.Catalog.Contracts.Dtos;
using System.Text.Json;

namespace OnlineStore.Catalog.Infrastructure.Messaging
{
    public class KafkaConsumer : IMessageConsumer
    {
        private readonly IKafkaConsumerFactory _kafkaConsumerFactory;
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public KafkaConsumer(
            IKafkaConsumerFactory kafkaConsumerFactory,
            ILogger<KafkaConsumer> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _kafkaConsumerFactory = kafkaConsumerFactory;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ConsumeAsync(string topic, CancellationToken cancellation)
        {
            using var consumer = _kafkaConsumerFactory.CreateConsumer<Null, string>();

            consumer.Subscribe(topic);

            while (!cancellation.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(TimeSpan.FromSeconds(5));

                if (consumeResult == null) continue;

                var productDtos = JsonSerializer.Deserialize<ShortProductDto[]>(consumeResult.Message.Value);

                using var scope = _serviceScopeFactory.CreateAsyncScope();
                var productService = scope.ServiceProvider.GetRequiredService<IProductService>();

                //TODO: добавить проверку на существование категории по названию
                await productService.AddProductsAsync(productDtos, cancellation);

                _logger.LogInformation($"Consume a message at offset: {consumeResult.Offset}");
            }
        }
    }
}
