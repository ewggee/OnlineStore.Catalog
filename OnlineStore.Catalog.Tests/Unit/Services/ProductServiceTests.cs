using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using OnlineStore.Catalog.Application.Abstractions;
using OnlineStore.Catalog.Application.Services;
using OnlineStore.Catalog.Contracts.Dtos;
using OnlineStore.Catalog.Contracts.Enums;
using OnlineStore.Catalog.Contracts.Options;
using OnlineStore.Catalog.Contracts.Requests;
using OnlineStore.Catalog.Domain.Entities;
using OnlineStore.Catalog.Domain.Exceptions;
using OnlineStore.Catalog.Domain.Repositories;
using OnlineStore.Catalog.Infrastructure.Specifications;
using OnlineStore.Catalog.Tests.Helpers;

namespace OnlineStore.Catalog.Tests.Unit.Services;

[TestFixture]
public class ProductServiceTests
{
    private const string ImagesUrl = "http://testimages/";
    private const int PageSize = 2;

    private ProductService _productService;

    private Mock<IProductRepository> _mockProductRepository;
    private Mock<ISpecificationFactory> _mockSpecificationFactory;
    private Mock<IMapper> _mockMapper;
    private Mock<IImageService> _mockImageService;
    private IFixture _fixture;

    [SetUp]
    public void Setup()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockSpecificationFactory = new Mock<ISpecificationFactory>();
        _mockMapper = new Mock<IMapper>();
        _mockImageService = new Mock<IImageService>();
        _fixture = new Fixture();
        
        _productService = new ProductService(
            _mockProductRepository.Object,
            _mockImageService.Object,
            _mockMapper.Object,
            _mockSpecificationFactory.Object,
            Mock.Of<TimeProvider>(),
            Options.Create(new ImageOptions { ImagesUrl = ImagesUrl }),
            Options.Create(new PaginationOptions { ProductsPageSize = PageSize })
        );
    }

    [Test]
    public async Task GetAsync_WhenProductExistsAndHasAdditionalImages_ReturnsProductWithImages()
    {
        var productId = 1;
        var mainImageId = 1;
        var mainImageUrl = ImageUrlWithId(mainImageId);
        var additionalImageUrl = ImageUrlWithId(2);

        var testProduct = ProductBuilder.Create()
            .WithId(productId)
            .WithImages(new List<ProductImage>
            {
                new() { Id = mainImageId },
                new() { Id = 2 }
            })
            .Build();

        var expectedDto = ProductBuilder.Create()
            .WithId(productId)
            .WithMainImageUrl(mainImageUrl)
            .WithImages(new List<ProductImage>
            {
                new() { Id = 2 }
            })
            .BuildAsDto(ImagesUrl);

        _mockProductRepository
            .Setup(x => x.GetAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(testProduct);

        _mockMapper
            .Setup(x => x.Map<ShortProductDto>(testProduct))
            .Returns(expectedDto);

        _mockImageService
            .Setup(x => x.ExtractImageId(mainImageUrl))
            .Returns(mainImageId);

        var actual = await _productService.GetAsync(productId, It.IsAny<CancellationToken>());

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual.Id, Is.EqualTo(productId));
        Assert.That(actual.ImagesUrls, Is.EquivalentTo(new[] { additionalImageUrl }));
    }

    [Test]
    public void GetAsync_WhenProductNotExists_ThrowsException()
    {
        var productId = 1;

        _mockProductRepository
            .Setup(x => x.GetAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _productService.GetAsync(productId, It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task GetAsync_WhenNoAdditionalImages_ReturnsProductWithoutAdditionalImages()
    {
        var productId = 1;
        var mainImageId = 1;
        var mainImageUrl = ImageUrlWithId(mainImageId);

        var testProduct = ProductBuilder.Create()
            .WithId(productId)
            .WithMainImageUrl(mainImageUrl)
            .Build();

        var expectedDto = ProductBuilder.Create()
            .WithId(productId)
            .WithMainImageUrl(mainImageUrl)
            .BuildAsDto(ImagesUrl);

        _mockProductRepository
            .Setup(x => x.GetAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(testProduct);

        _mockMapper
            .Setup(x => x.Map<ShortProductDto>(testProduct))
            .Returns(expectedDto);

        _mockImageService
            .Setup(x => x.ExtractImageId(mainImageUrl))
            .Returns(mainImageId);

        var actual = await _productService.GetAsync(productId, It.IsAny<CancellationToken>());

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual.Id, Is.EqualTo(productId));
        Assert.That(actual.MainImageUrl, Is.EqualTo(expectedDto.MainImageUrl));
        Assert.That(actual.ImagesUrls, Is.Empty);
    }

    [Test]
    public async Task GetAllAsync_WhenCategoryContainsProducts_ReturnsProductList()
    {
        // TODO: отрефакторить
        var categoryId = 1;
        var pageNumber = 2;
        var request = new GetProductsRequest 
        {
            CategoryId = categoryId, 
            Page = pageNumber, 
            Sort = ProductsSortEnum.PriceAsc 
        };
        var categoryDto = new CategoryDto { Id = categoryId };
        var products = _fixture.Build<Product>()
            .Without(p => p.Category)
            .Without(p => p.Images)
            .CreateMany(4)
            .ToList();

        _mockProductRepository
            .Setup(x => x.GetProductsTotalCountAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(products.Count);

        var specification = new ProductSpecification(request, PageSize);
        _mockSpecificationFactory
            .Setup(x => x.CreateProductSpecification(request, PageSize))
            .Returns(specification);

        var repositoryPaginatedResult = products
            .Skip((pageNumber - 1) * PageSize)
            .Take(PageSize).ToList();
        _mockProductRepository
            .Setup(x => x.GetAllAsync(specification, CancellationToken.None))
            .ReturnsAsync(repositoryPaginatedResult);

        var mappedResult = repositoryPaginatedResult
            .Select(p => new ShortProductDto
            {
                Id = p.Id
            }).ToList();
        _mockMapper
            .Setup(x => x.Map<List<ShortProductDto>>(repositoryPaginatedResult))
            .Returns(mappedResult);

        var actual = await _productService.GetAllAsync(request, categoryDto, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(actual.Page, Is.EqualTo(pageNumber));
            Assert.That(actual.PageSize, Is.EqualTo(PageSize));
            Assert.That(actual.TotalCount, Is.EqualTo(products.Count));
            Assert.That(actual.Result, Is.EqualTo(mappedResult));
            Assert.That(actual.CategoryDto, Is.EqualTo(categoryDto));
            Assert.That(actual.Sorting, Is.EqualTo(request.Sort));
        });
    }

    [Test]
    public async Task GetAllAsync_WhenCategoryNotContainsProducts_ReturnsEmptyList()
    {
        var categoryId = 1;
        var request = new GetProductsRequest { CategoryId = categoryId };
        var categoryDto = new CategoryDto { Id = categoryId };

        _mockProductRepository
            .Setup(x => x.GetProductsTotalCountAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        var actual = await _productService.GetAllAsync(request, categoryDto, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(actual.Page, Is.EqualTo(1));
            Assert.That(actual.TotalCount, Is.EqualTo(0));
            Assert.That(actual.Result, Is.Empty);
            Assert.That(actual.CategoryDto, Is.EqualTo(categoryDto));
        });
    }

    private string ImageUrlWithId(int imageId) => ImagesUrl + imageId;
}