using OnlineStore.Catalog.Application.Abstractions;
using OnlineStore.Catalog.Contracts.Dtos;
using OnlineStore.Catalog.Contracts.Helpers;

namespace OnlineStore.Catalog.Application.Services
{
    public class CachingCategoryService : ICategoryService
    {
        private readonly ICacheService _cacheService;
        private readonly ICategoryService _categoryService;

        public CachingCategoryService(
            ICacheService cacheService, 
            ICategoryService categoryService)
        {
            _cacheService = cacheService;
            _categoryService = categoryService;
        }

        public async Task<IReadOnlyList<CategoryDto>> GetMainCategoriesAsync(CancellationToken cancellation)
        {
            var mainCategorires = await _cacheService.GetOrSetAsUnlimitedAsync(
                key: CategoryCacheKeysHelper.MainCategories,
                func: async () => await _categoryService.GetMainCategoriesAsync(cancellation));

            return mainCategorires!;
        }

        public async Task<CategoryDto> GetAsync(int categoryId, CancellationToken cancellation)
        {
            var category = await _cacheService.GetOrSetAsUnlimitedAsync(
                key: CategoryCacheKeysHelper.Category(categoryId),
                func: async () => await _categoryService.GetAsync(categoryId, cancellation));

            return category!;
        }

        public async Task<CategoryWithSubcategoriesDto> GetCategoryWithSubcategoriesAsync(int categoryId, CancellationToken cancellation)
        {
            var categoryWithSubcategories = await _cacheService.GetOrSetAsUnlimitedAsync(
                key: CategoryCacheKeysHelper.CategoryWithSubcategories(categoryId),
                func: async () => await _categoryService.GetCategoryWithSubcategoriesAsync(categoryId, cancellation));

            return categoryWithSubcategories!;
        }

        public async Task<IReadOnlyList<CategoryDto>> GetNavigationCategoriesAsync(CancellationToken cancellation, int? categoryId = null)
        {
            var navigationCategories = await _cacheService.GetOrSetAsUnlimitedAsync(
                key: CategoryCacheKeysHelper.NavigationCategories(categoryId),
                func: async () => await _categoryService.GetNavigationCategoriesAsync(cancellation, categoryId));

            return navigationCategories!;
        }

        public async Task<IReadOnlyList<CategoryDto>> GetWithoutSubcategories(CancellationToken cancellation)
        {
            var independentCategories = await _cacheService.GetOrSetAsUnlimitedAsync(
                key: CategoryCacheKeysHelper.IndependentCategories,
                func: async () => await _categoryService.GetWithoutSubcategories(cancellation));

            return independentCategories!;
        }

        public async Task<int> AddAsync(CategoryDto categoryDto, CancellationToken cancellation)
        {
            var categoryId = await _categoryService.AddAsync(categoryDto, cancellation);

            return categoryId;
        }

        public async Task UpdateAsync(CategoryDto categoryDto, CancellationToken cancellation)
        {
            await _categoryService.UpdateAsync(categoryDto, cancellation);

            await _cacheService.RefreshAsync(
                key: CategoryCacheKeysHelper.Category(categoryDto.Id),
                value: categoryDto);
        }

        public async Task DeleteAsync(int categoryId, CancellationToken cancellation)
        {
            await _categoryService.DeleteAsync(categoryId, cancellation);

            await _cacheService.RemoveAsync(CategoryCacheKeysHelper.Category(categoryId));
        }

        public async Task<bool> IsCategoryHasSubcategoriesAsync(int categoryId, CancellationToken cancellation)
        {
            return await _categoryService.IsCategoryHasSubcategoriesAsync(categoryId, cancellation);
        }

        public async Task<bool> IsCategoryHasProductsAsync(int categoryId, CancellationToken cancellation)
        {
            return await _categoryService.IsCategoryHasProductsAsync(categoryId, cancellation);
        }
    }
}
