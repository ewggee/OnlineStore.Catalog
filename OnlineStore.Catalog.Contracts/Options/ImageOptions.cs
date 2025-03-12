using Microsoft.Extensions.Configuration;

namespace OnlineStore.Catalog.Contracts.Options;

public sealed class ImageOptions
{
    [ConfigurationKeyName("ImagesUrl")]
    public string ImagesUrl { get; set; }
}
