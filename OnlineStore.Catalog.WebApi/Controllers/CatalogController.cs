using Microsoft.AspNetCore.Mvc;
using OnlineStore.Catalog.Application.Abstractions;
using OnlineStore.Catalog.Contracts.Requests;

namespace OnlineStore.Catalog.WebApi.Controllers;

[Route("api/catalog")]
public class CatalogController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;

    public CatalogController(
        ICategoryService categoryService,
        IProductService productService)
    {
        _categoryService = categoryService;
        _productService = productService;
    }

    [HttpGet("MainCategories")]
    public async Task<IActionResult> GetMainCategories(CancellationToken cancellation)
    {
        var mainCategories = await _categoryService.GetMainCategoriesAsync(cancellation);

        return Ok(mainCategories);
    }

    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetCategory(int categoryId, CancellationToken cancellation)
    {
        var existingCategory = await _categoryService.GetAsync(categoryId, cancellation);
        if (existingCategory == null)
        {
            return NotFound();
        }

        return Ok(existingCategory);
    }

    [HttpGet("{categoryId}/SubCategories")]
    public async Task<IActionResult> GetSubcategories(int categoryId, CancellationToken cancellation)
    {
        var existingCategory = await _categoryService.GetAsync(categoryId, cancellation);
        if (existingCategory == null)
        {
            return NotFound();
        }

        if (!await _categoryService.IsCategoryHasSubcategoriesAsync(existingCategory.Id))
        {
            return BadRequest();
        }

        var subCategories = await _categoryService.GetSubcategoriesByIdAsync(categoryId, cancellation);

        return Ok(new { category = existingCategory, subCategories });
    }

    [HttpGet("{categoryId}/Products")]
    public async Task<IActionResult> GetProducts([FromRoute] int categoryId, GetProductsRequest request, CancellationToken cancellation)
    {
        var existingCategory = await _categoryService.GetAsync(request.CategoryId, cancellation);
        if (existingCategory == null)
        {
            return NotFound();
        }

        if (await _categoryService.IsCategoryHasSubcategoriesAsync(existingCategory.Id))
        {
            return BadRequest();
        }

        var products = await _productService.GetAllAsync(request, existingCategory, cancellation);

        return Ok(products);
    }

    [HttpGet("Product/{productId}")]
    public async Task<IActionResult> GetProduct(int productId, CancellationToken cancellation)
    {
        var existingProduct = await _productService.GetAsync(productId, cancellation);
        if (existingProduct == null)
        {
            return NotFound();
        }

        return Ok(existingProduct);
    }
}