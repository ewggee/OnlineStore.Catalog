using AutoMapper;
using Microsoft.Extensions.Options;
using OnlineStore.Catalog.Application.Abstractions;
using OnlineStore.Catalog.Contracts.Dtos;
using OnlineStore.Catalog.Contracts.Options;
using OnlineStore.Catalog.Contracts.Requests;
using OnlineStore.Catalog.Domain.Entities;
using OnlineStore.Catalog.Domain.Repositories;

namespace OnlineStore.Catalog.Application.Services;

/// <summary>
/// Сервис по работе с товарами.
/// </summary>
public sealed class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;
    private readonly ISpecificationFactory _specificationFactory;
    private readonly TimeProvider _timeProvider;
    private readonly ImageOptions _imageOptions;
    private readonly int _pageSize;

    public ProductService(
        IProductRepository productRepository,
        IImageService imageService,
        IMapper mapper,
        ISpecificationFactory specificationFactory,
        TimeProvider timeProvider,
        IOptions<ImageOptions> imageOptions,
        IOptions<PaginationOptions> paginationOptions)
    {
        _productRepository = productRepository;
        _imageService = imageService;
        _mapper = mapper;
        _specificationFactory = specificationFactory;
        _timeProvider = timeProvider;
        _imageOptions = imageOptions.Value;
        _pageSize = paginationOptions.Value.ProductsPageSize;
    }

    /// <inheritdoc/>
    public async Task<ShortProductDto?> GetAsync(int productId, CancellationToken cancellation)
    {
        var existingProduct = await _productRepository.GetAsync(productId);
        //todo: маппинг возможен после загрузки изображений
        var productDto = _mapper.Map<ShortProductDto>(existingProduct);

        if (existingProduct != null)
        {
            // Загрузка изображений
            var imagesUrls = new List<string>();
            var mainImageId = _imageService.ExtractImageId(productDto.MainImageUrl);
            foreach (var image in existingProduct.Images)
            {
                if (image.Id != mainImageId)
                {
                    imagesUrls.Add(string.Concat(_imageOptions.ImagesUrl, image.Id));
                }
            }
            productDto.ImagesUrls = imagesUrls.ToArray();
        }

        return productDto;
    }

    /// <inheritdoc/>
    public async Task<ProductsListDto> GetAllAsync(GetProductsRequest request, CategoryDto categoryDto, CancellationToken cancellation)
    {
        var totalCount = await _productRepository.GetProductsTotalCountAsync(request.CategoryId, cancellation);

        if (totalCount == 0)
        {
            return new ProductsListDto
            {
                Page = 1,
                PageSize = _pageSize,
                TotalCount = totalCount,
                Result = [],
                CategoryDto = categoryDto,
                Sorting = request.Sort
            };
        }

        var specification = _specificationFactory.CreateProductSpecification(request, _pageSize);

        var products = await _productRepository.GetAllAsync(specification, cancellation);
        var productsDtos = _mapper.Map<List<ShortProductDto>>(products);

        return new ProductsListDto
        {
            Page = request.Page,
            PageSize = _pageSize,
            TotalCount = totalCount,
            Result = productsDtos,
            CategoryDto = categoryDto,
            Sorting = request.Sort
        };
    }

    /// <inheritdoc/>
    public async Task<List<ShortProductDto>> GetAllAsync(int categoryId, CancellationToken cancellation)
    {
        var productsInCategory = await _productRepository.GetAllAsync(categoryId, cancellation);

        return _mapper.Map<List<ShortProductDto>>(productsInCategory);
    }

    /// <inheritdoc/>
    public async Task<List<ShortProductDto>> GetAllAsync(int[] productsIds, CancellationToken cancellation)
    {
        var products = await _productRepository.GetAllAsync(productsIds, cancellation);

        return _mapper.Map<List<ShortProductDto>>(products);
    }

    /// <inheritdoc/>
    public async Task<int> AddProductAsync(ShortProductDto productDto, CancellationToken cancellation)
    {
        var product = _mapper.Map<Product>(productDto);

        product.Images = await _imageService.SaveProductImagesAsync(productDto.ImagesUrls, product, cancellation);
        product.CreatedAt = _timeProvider.GetUtcNow().UtcDateTime;

        var productId = await _productRepository.AddAsync(product, cancellation);
        return productId;
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(ShortProductDto productDto, CancellationToken cancellation)
    {
        var product = _mapper.Map<Product>(productDto);
        product.Images = await _imageService.SaveProductImagesAsync(productDto.ImagesUrls, product, cancellation);
        product.UpdatedAt = _timeProvider.GetUtcNow().UtcDateTime;
        
        await _productRepository.UpdateAsync(product, cancellation);
    }

    /// <inheritdoc/>
    public async Task UpdateProductsCountAsync(ShortProductDto[] productDtos, CancellationToken cancellation)
    {
        var products = _mapper.Map<Product[]>(productDtos);

        await _productRepository.UpdateProductsCountAsync(products, cancellation);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int productId, CancellationToken cancellation)
    {
        var product = await _productRepository.GetAsync(productId);
        product!.IsDeleted = true;

        await _productRepository.DeleteAsync(product, cancellation);
    }
}
