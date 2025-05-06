using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OnlineStore.Catalog.Application.Abstractions;
using OnlineStore.Catalog.Contracts.Dtos;
using OnlineStore.Catalog.Contracts.Options;
using OnlineStore.Catalog.Domain.Entities;
using OnlineStore.Catalog.Domain.Repositories;

namespace OnlineStore.Catalog.Application.Services;

public sealed class ImageService : IImageService
{
    private readonly IImageRepository _imageRepository;
    private readonly IMapper _mapper;
    private readonly ImageOptions _imageOptions;

    public ImageService(
        IImageRepository imageRepository,
        IMapper mapper,
        IOptions<ImageOptions> imageOptions)
    {
        _imageRepository = imageRepository;
        _mapper = mapper;
        _imageOptions = imageOptions.Value;
    }

    /// <inheritdoc/>
    public async Task<ImageDto> GetAsync(int imageId, CancellationToken cancellation)
    {
        var image = await _imageRepository.GetAsync(imageId, cancellation);

        return _mapper.Map<ImageDto>(image);
    }

    /// <inheritdoc/>
    public string[] GetImagesUrls(ProductImage[] images)
    {
        if (images.Length == 0)
        {
            return [];
        }

        var urls = new List<string>(images.Length);
        foreach (var image in images)
        {
            urls.Add(string.Concat(_imageOptions.ImagesUrl, image.Id));
        }

        return urls.ToArray();
    }

    /// <inheritdoc/>
    public async Task<string> SaveImageAsync(IFormFile imageFile, CancellationToken cancellation)
    {
        var productImage = new ProductImage
        {
            Name = imageFile.FileName,
            Content = await GetByteArrayAsync(imageFile, cancellation),
            ContentType = imageFile.ContentType
        };

        var imageId = await _imageRepository.SaveAsync(productImage, cancellation);
        
        return string.Concat(_imageOptions.ImagesUrl, imageId);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<string>> SaveImagesAsync(List<IFormFile> ImageFiles, CancellationToken cancellation)
    {
        var generatedURLs = new List<string>(ImageFiles.Count);
        foreach (var imageFile in ImageFiles)
        {
            var imageUrl = await SaveImageAsync(imageFile, cancellation);
            generatedURLs.Add(imageUrl);
        }

        return generatedURLs.ToArray();
    }

    /// <inheritdoc/>
    public async Task<ProductImage[]> SaveProductImagesAsync(string[] imagesUrls, Product product, CancellationToken cancellation)
    {
        var result = new List<ProductImage>();

        var imageId = ExtractImageId(product.ImageUrl);
        var existingImage = await _imageRepository.GetAsync(imageId, cancellation);

        existingImage.Product = product;

        result.Add(existingImage);

        foreach (var imageUrl in imagesUrls)
        {
            imageId = ExtractImageId(imageUrl);
            existingImage = await _imageRepository.GetAsync(imageId, cancellation);

            existingImage.Product = product;

            result.Add(existingImage);
        }
        
        return result.ToArray();
    }

    /// <inheritdoc/>
    public int ExtractImageId(string url)
    {
        var lastSlashIndex = url.LastIndexOf('/');
        
        var stringId = url.Substring(lastSlashIndex + 1);

        return int.Parse(stringId);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int imageId, CancellationToken cancellation)
    {
        var image = new ProductImage { Id = imageId };

        await _imageRepository.DeleteAsync(image, cancellation);
    }

    private async Task<byte[]> GetByteArrayAsync(IFormFile imageFile, CancellationToken cancellation)
    {
        using var memoryStream = new MemoryStream();
        await imageFile.CopyToAsync(memoryStream, cancellation);
        
        return memoryStream.ToArray();
    }
}
