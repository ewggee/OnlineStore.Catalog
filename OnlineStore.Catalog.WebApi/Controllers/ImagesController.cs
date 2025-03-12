using Microsoft.AspNetCore.Mvc;
using OnlineStore.Catalog.Application.Abstractions;
using OnlineStore.Catalog.Contracts.Consts;
using OnlineStore.Catalog.Contracts.Requests;

namespace OnlineStore.Catalog.WebApi.Controllers;

public class ImagesController : Controller
{
    private readonly IImageService _imageService;

    public ImagesController(
        IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpGet("images/{imageId}")]
    public async Task<IActionResult> GetImage([FromRoute] int imageId, CancellationToken cancellation)
    {
        var image = await _imageService.GetAsync(imageId, cancellation);

        return File(image.Data, image.ContentType);
    }

    [HttpPost]
    public async Task<IActionResult> UploadImages(List<IFormFile> imageFiles, CancellationToken cancellation)
    {
        var result = new List<IFormFile>();
        foreach (var imageFile in imageFiles)
        {
            var isImageContainsContent = imageFile.Length > 0;

            var isImageValidContentType =
                imageFile.ContentType == ImageTypeConsts.Png ||
                imageFile.ContentType == ImageTypeConsts.Jpg ||
                imageFile.ContentType == ImageTypeConsts.Jpeg;

            if (!(isImageContainsContent && isImageValidContentType))
                continue;

            result.Add(imageFile);
        }

        var imageUrls = await _imageService.SaveImagesAsync(result, cancellation);

        return Json(imageUrls);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteImage([FromBody] DeleteImageRequest deleteImageRequest, CancellationToken cancellation)
    {
        var imageId = _imageService.ExtractImageId(deleteImageRequest.imageUrl);

        await _imageService.DeleteAsync(imageId, cancellation);

        return Ok();
    }
}
