using Microsoft.AspNetCore.Mvc;
using OnlineStore.Catalog.Application.Abstractions;
using OnlineStore.Catalog.Contracts.Requests;

namespace OnlineStore.Catalog.WebApi.Controllers
{
    [ApiController]
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
            var category = await _categoryService.GetAsync(categoryId, cancellation);

            return Ok(category);
        }

        [HttpGet("{categoryId}/Subcategories")]
        public async Task<IActionResult> GetSubcategories([FromRoute] int categoryId, CancellationToken cancellation)
        {
            var categoryWithSubcategories = await _categoryService.GetCategoryWithSubcategoriesAsync(categoryId, cancellation);

            if (!categoryWithSubcategories.Subcategories.Any())
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "No subcategories found.",
                    Instance = HttpContext.Request.Path
                });
            }

            return Ok(categoryWithSubcategories);
        }

        [HttpGet("{categoryId}/Products")]
        public async Task<IActionResult> GetProducts([FromRoute] int categoryId, GetProductsRequest request, CancellationToken cancellation)
        {
            var category = await _categoryService.GetAsync(categoryId, cancellation);

            if (await _categoryService.IsCategoryHasSubcategoriesAsync(category.Id, cancellation))
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Category has subcategories. Products cannot be retrieved.",
                    Instance = HttpContext.Request.Path
                });
            }

            var products = await _productService.GetAllAsync(request, category, cancellation);

            return Ok(products);
        }

        [HttpGet("Product/{productId}")]
        public async Task<IActionResult> GetProduct(int productId, CancellationToken cancellation)
        {
            var product = await _productService.GetAsync(productId, cancellation);

            return Ok(product);
        }
    }
}