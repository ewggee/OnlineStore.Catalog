using AutoMapper;
using OnlineStore.Catalog.Application.Abstractions;
using OnlineStore.Catalog.Contracts.Dtos;
using OnlineStore.Catalog.Domain.Entities;
using OnlineStore.Catalog.Domain.Repositories;

namespace OnlineStore.Catalog.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<CategoryDto> GetAsync(int categoryId, CancellationToken cancellation)
    {
        var categoryDto = await _categoryRepository.GetAsync(categoryId);

        return _mapper.Map<CategoryDto>(categoryDto);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<CategoryDto>> GetMainCategoriesAsync(CancellationToken cancellation)
    {
        var mainCategories = await _categoryRepository.GetAllAsync(c => c.ParentCategoryId == null, cancellation);

        return _mapper.Map<IReadOnlyList<CategoryDto>>(mainCategories);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<CategoryDto>> GetSubcategoriesByIdAsync(int categoryId, CancellationToken cancellation)
    {
        var subCategories = await _categoryRepository.GetAllAsync(c => c.ParentCategoryId == categoryId, cancellation);

        return _mapper.Map<CategoryDto[]>(subCategories);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<CategoryDto>> GetNavigationCategoriesAsync(CancellationToken cancellation, int? categoryId = null)
    {
        var navigationCategories = await _categoryRepository.GetNavigationCategoriesAsync(cancellation, categoryId);

        return _mapper.Map<CategoryDto[]>(navigationCategories);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<CategoryDto>> GetWithoutSubcategories(CancellationToken cancellation)
    {
        var independentCategories = await _categoryRepository.GetWithoutSubsAsync(cancellation);

        return _mapper.Map<CategoryDto[]>(independentCategories);
    }

    /// <inheritdoc/>
    public async Task AddAsync(CategoryDto categoryDto, CancellationToken cancellation)
    {
        var category = _mapper.Map<Category>(categoryDto);

        await _categoryRepository.AddAsync(category, cancellation);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(CategoryDto categoryDto, CancellationToken cancellation)
    {
        var category = _mapper.Map<Category>(categoryDto);

        await _categoryRepository.UpdateAsync(category, cancellation);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int categoryId, CancellationToken cancellation)
    {
        var category = new Category { Id = categoryId };

        await _categoryRepository.DeleteAsync(category, cancellation);
    }

    /// <inheritdoc/>
    public async Task<bool> IsCategoryHasSubcategoriesAsync(int categoryId, CancellationToken cancellation)
    {
        return await _categoryRepository.IsCategoryHasSubcategoriesAsync(categoryId, cancellation);
    }

    /// <inheritdoc/>
    public async Task<bool> IsCategoryHasProductsAsync(int categoryId, CancellationToken cancellation)
    {
        return await _productRepository.IsCategoryHasProductsAsync(categoryId, cancellation);
    }
}