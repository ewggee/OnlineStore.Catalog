using OnlineStore.Catalog.Application.Abstractions;
using OnlineStore.Catalog.Contracts.Dtos;
using OnlineStore.Catalog.Contracts.Helpers;
using OnlineStore.Catalog.Contracts.Requests;

namespace OnlineStore.Catalog.Application.Services
{
    public class CachingProductService : IProductService
    {
        private readonly ICacheService _cacheService;
        private readonly IProductService _productService;

        private readonly TimeSpan _defaultCacheLifetime = TimeSpan.FromMinutes(5);

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
                key: ProductCacheKeysHelper.ProductById(productId),
                func: async () => await _productService.GetAsync(productId, cancellation),
                lifetime: _defaultCacheLifetime);

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

        public Task<ProductsListDto> GetAllAsync(GetProductsRequest request, CategoryDto categoryDto, CancellationToken cancellation)
        {
            var products = _cacheService.GetOrSetAsync(
                key: ProductCacheKeysHelper.ProductsInCategory(request.CategoryId),
                func: async () => await _productService.GetAllAsync(request, categoryDto, cancellation),
                lifetime: _defaultCacheLifetime);

            return products!;
        }

        public async Task<int> AddProductAsync(ShortProductDto productDto, CancellationToken cancellation)
        {
            return await _productService.AddProductAsync(productDto, cancellation);
        }

        public async Task AddProductsAsync(ShortProductDto[] productDtos, CancellationToken cancellation)
        {
            await _productService.AddProductsAsync(productDtos, cancellation);
        }

        public async Task UpdateAsync(ShortProductDto productDto, CancellationToken cancellation)
        {
            await _productService.UpdateAsync(productDto, cancellation);

            await _cacheService.RemoveAsync(ProductCacheKeysHelper.ProductById(productDto.Id));
        }

        public async Task UpdateProductsCountAsync(ShortProductDto[] productsDtos, CancellationToken cancellation)
        {
            await _productService.UpdateProductsCountAsync(productsDtos, cancellation);

            var keys = productsDtos
                .Select(p => ProductCacheKeysHelper.ProductById(p.Id))
                .ToArray();

            await _cacheService.RemoveRangeAsync(keys);
        }
        public async Task DeleteAsync(int productId, CancellationToken cancellation)
        {
            await _productService.DeleteAsync(productId, cancellation);

            await _cacheService.RemoveAsync(ProductCacheKeysHelper.ProductById(productId));
        }
    }
}
