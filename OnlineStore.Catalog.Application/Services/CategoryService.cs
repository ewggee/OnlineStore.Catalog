using AutoMapper;
using OnlineStore.Catalog.Application.Abstractions;
using OnlineStore.Catalog.Contracts.Dtos;
using OnlineStore.Catalog.Contracts.Helpers;
using OnlineStore.Catalog.Domain.Entities;
using OnlineStore.Catalog.Domain.Exceptions;
using OnlineStore.Catalog.Domain.Repositories;

namespace OnlineStore.Catalog.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<CategoryDto> GetAsync(int categoryId, CancellationToken cancellation)
    {
        var category = await _categoryRepository.GetAsync(categoryId, cancellation)
            ?? throw new EntityNotFoundException(ExceptionMessagesHelper.EntityNotFound<Category>(categoryId));

        return _mapper.Map<CategoryDto>(category);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<CategoryDto>> GetMainCategoriesAsync(CancellationToken cancellation)
    {
        var mainCategories = await _categoryRepository.GetAllAsync(c => c.ParentCategoryId == null, cancellation);

        return _mapper.Map<IReadOnlyList<CategoryDto>>(mainCategories);
    }

    /// <inheritdoc/>
    public async Task<CategoryWithSubcategoriesDto> GetCategoryWithSubcategoriesAsync(int categoryId, CancellationToken cancellation)
    {
        var category = await _categoryRepository.GetAsync(categoryId, cancellation) 
            ?? throw new EntityNotFoundException(ExceptionMessagesHelper.EntityNotFound<Category>(categoryId));

        var subCategories = await _categoryRepository.GetAllAsync(c => c.ParentCategoryId == category.Id, cancellation);

        var categoryDto = _mapper.Map<CategoryDto>(category);
        var subCategoriesDtos = _mapper.Map<IReadOnlyList<CategoryDto>>(subCategories);

        return new CategoryWithSubcategoriesDto
        {
            Category = categoryDto,
            Subcategories = subCategoriesDtos
        };
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<CategoryDto>> GetNavigationCategoriesAsync(CancellationToken cancellation, int? categoryId = null)
    {
        var navigationCategories = await _categoryRepository.GetNavigationCategoriesAsync(cancellation, categoryId);

        return _mapper.Map<IReadOnlyList<CategoryDto>>(navigationCategories);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<CategoryDto>> GetWithoutSubcategories(CancellationToken cancellation)
    {
        var independentCategories = await _categoryRepository.GetWithoutSubsAsync(cancellation);

        return _mapper.Map<IReadOnlyList<CategoryDto>>(independentCategories);
    }

    /// <inheritdoc/>
    public async Task<int> AddAsync(CategoryDto categoryDto, CancellationToken cancellation)
    {
        var category = _mapper.Map<Category>(categoryDto);

        var categoryId = await _categoryRepository.AddAsync(category, cancellation);

        return categoryId;
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
        var category = await _categoryRepository.GetAsync(categoryId, cancellation)
            ?? throw new EntityNotFoundException(ExceptionMessagesHelper.EntityNotFound<Category>(categoryId));

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