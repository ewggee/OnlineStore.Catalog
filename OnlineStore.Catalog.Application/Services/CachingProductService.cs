using OnlineStore.Catalog.Application.Abstractions;
using OnlineStore.Catalog.Contracts.Common;
using OnlineStore.Catalog.Contracts.Dtos;
using OnlineStore.Catalog.Contracts.Requests;

namespace OnlineStore.Catalog.Application.Services
{
    public class CachingProductService : IProductService
    {
        private readonly ICacheService _cacheService;
        private readonly IProductService _productService;

        public CachingProductService(
            ICacheService cacheService,
            IProductService productService)
        {
            _cacheService = cacheService;
            _productService = productService;
        }

        public async Task<ShortProductDto?> GetAsync(int productId, CancellationToken cancellation)
        {
            var product = await _cacheService.GetOrSetAsync(
                key: CacheKeysHelper.ProductById(productId.ToString()),
                func: async () => await _productService.GetAsync(productId, cancellation));

            return product;
        }

        public async Task<List<ShortProductDto>> GetAllAsync(int categoryId, CancellationToken cancellation)
        {
            return await _productService.GetAllAsync(categoryId, cancellation);
        }

        public async Task<List<ShortProductDto>> GetAllAsync(int[] productsIds, CancellationToken cancellation)
        {
            return await _productService.GetAllAsync(productsIds, cancellation);
        }

        public async Task<ProductsListDto> GetAllAsync(GetProductsRequest request, CategoryDto categoryDto, CancellationToken cancellation)
        {
            var products = await _cacheService.GetOrSetAsync(
                key: $"ProductsListDto:{request.CategoryId}",
                func: async () => await _productService.GetAllAsync(request, categoryDto, cancellation),
                lifetime: TimeSpan.FromMinutes(5));

            return products;
        }

        public async Task AddProductAsync(ShortProductDto productDto, CancellationToken cancellation)
        {
            await _productService.AddProductAsync(productDto, cancellation);
        }

        public async Task UpdateAsync(ShortProductDto productDto, CancellationToken cancellation)
        {
            await _productService.UpdateAsync(productDto, cancellation);
        }

        public async Task UpdateProductsCountAsync(ShortProductDto[] productsDtos, CancellationToken cancellation)
        {
            await _productService.UpdateProductsCountAsync(productsDtos, cancellation);
        }
        public async Task DeleteAsync(int productId, CancellationToken cancellation)
        {
            await _productService.DeleteAsync(productId, cancellation);
        }
    }
}
