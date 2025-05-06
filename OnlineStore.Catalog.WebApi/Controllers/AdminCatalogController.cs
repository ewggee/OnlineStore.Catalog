using Microsoft.AspNetCore.Mvc;
using OnlineStore.Catalog.Application.Abstractions;
using OnlineStore.Catalog.Contracts.Dtos;
using OnlineStore.Catalog.Contracts.Responses;

namespace OnlineStore.Catalog.WebApi.Controllers
{
    [ApiController]
    [Route("admin/api/catalog")]
    public class AdminCatalogController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public AdminCatalogController(
            ICategoryService categoryService,
            IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        #region for Category

        [HttpGet("navigationCategories")]
        public async Task<IActionResult> GetNavigationCategories(CancellationToken cancellation, int? parentCategoryId = null)
        {
            var categories = await _categoryService.GetNavigationCategoriesAsync(cancellation, parentCategoryId);

            return Ok(categories);
        }

        [HttpGet("withoutSubcategories")]
        public async Task<IActionResult> GetWithoutSubcategories(CancellationToken cancellation)
        {
            var categories = await _categoryService.GetWithoutSubcategories(cancellation);

            return Ok(categories);
        }

        [HttpPost("category/add")]
        public async Task<IActionResult> AddCategory(CategoryDto categoryDto, CancellationToken cancellation)
        {
            var categoryId = await _categoryService.AddAsync(categoryDto, cancellation);

            return Ok(categoryId);
        }

        [HttpGet("category/{categoryId}/update")]
        public async Task<IActionResult> UpdateCategory(int categoryId, CancellationToken cancellation)
        {
            var existingCategoryDto = await _categoryService.GetAsync(categoryId, cancellation);
            if (existingCategoryDto == null)
            {
                return NotFound();
            }

            var independentCategories = await _categoryService.GetNavigationCategoriesAsync(cancellation, categoryId);

            var isHasSubcategories = await _categoryService.IsCategoryHasSubcategoriesAsync(categoryId, cancellation);
            var isHasProducts = await _categoryService.IsCategoryHasProductsAsync(categoryId, cancellation);

            var response = new UpdateCategoryResponse
            {
                Category = existingCategoryDto,
                SelectaleCategories = independentCategories,
                IsHasProducts = isHasProducts,
                IsHasSubcategories = isHasSubcategories
            };

            return Ok(response);
        }

        [HttpPost("category/update")]
        //[HttpPost("category/{categoryId}/update")]
        public async Task<IActionResult> UpdateCategory(CategoryDto categoryDto, CancellationToken cancellation)
        {
            await _categoryService.UpdateAsync(categoryDto, cancellation);

            return Ok();
        }

        [HttpPost("category/{categoryId}/delete")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int categoryId, CancellationToken cancellation)
        {
            await _categoryService.DeleteAsync(categoryId, cancellation);

            return Ok();
        }

        #endregion

        #region for Product

        [HttpPost("product/add")]
        
        public async Task<IActionResult> AddProduct(ShortProductDto shortProductDto, CancellationToken cancellation)
        {
            var productId = await _productService.AddProductAsync(shortProductDto, cancellation);

            return Ok(productId);
        }

        [HttpPost("product/update")]
        //[HttpPost("product/{productId}/update")]
        public async Task<IActionResult> UpdateProduct(ShortProductDto shortProductDto, CancellationToken cancellation)
        {
            await _productService.UpdateAsync(shortProductDto, cancellation);

            return Ok();
        }

        [HttpPost("product/{productId}/delete")]
        public async Task<IActionResult> DeleteProduct(int productId, CancellationToken cancellation)
        {
            var product = await _productService.GetAsync(productId, cancellation);
            if (product == null)
            {
                return NotFound();
            }

            await _productService.DeleteAsync(productId, cancellation);

            return Ok();
        }

        #endregion
    }
}
