namespace OnlineStore.Catalog.Application.Abstractions
{
    public interface IMessageConsumer
    {
        Task ConsumeAsync(string topic, CancellationToken cancellation);
    }
}
