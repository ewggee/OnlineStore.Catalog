namespace OnlineStore.Catalog.Contracts.Options
{
    public class KafkaOptions
    {
        public string BootstrapServers { get; set; }
        public string GroupId { get; set; }
    }
}
