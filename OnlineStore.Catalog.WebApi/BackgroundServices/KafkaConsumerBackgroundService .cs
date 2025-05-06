using OnlineStore.Catalog.Application.Abstractions;

namespace OnlineStore.Catalog.WebApi.BackgroundServices
{
    public class KafkaConsumerJob : BackgroundService
    {
        private readonly IMessageConsumer _messageConsumer;

        public KafkaConsumerJob(IMessageConsumer messageConsumer)
        {
            _messageConsumer = messageConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _messageConsumer.ConsumeAsync("test-topic", stoppingToken);
        }
    }
}
