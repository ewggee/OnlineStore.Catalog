using OnlineStore.Catalog.Contracts.Dtos;
using OnlineStore.Catalog.Domain.Entities;

namespace OnlineStore.Catalog.Tests.Helpers
{
    public class ProductBuilder
    {
        private readonly Product _product = new();

        // Статический метод для начала построения (опционально)
        public static ProductBuilder Create() => new();

        public ProductBuilder WithId(int id)
        {
            _product.Id = id;
            return this;
        }

        public ProductBuilder WithName(string name)
        {
            _product.Name = name;
            return this;
        }

        public ProductBuilder WithMainImageUrl(string mainImageUrl)
        {
            _product.ImageUrl = mainImageUrl;
            return this;
        }

        public ProductBuilder WithImages(ICollection<ProductImage> images)
        {
            _product.Images = images;
            return this;
        }

        public Product Build() => _product;

        public ShortProductDto BuildAsDto(string imagesUrl) => new()
        {
            Id = _product.Id,
            Name = _product.Name,
            MainImageUrl = _product.ImageUrl!,
            ImagesUrls = _product.Images.Select(i => imagesUrl + i.Id).ToArray()
        };
    }
}
