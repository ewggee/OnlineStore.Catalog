using OnlineStore.Catalog.Contracts.Dtos;

namespace OnlineStore.Catalog.Contracts.Responses
{
    public class UpdateCategoryResponse
    {
        public CategoryDto Category { get; set; }
        public IReadOnlyList<CategoryDto> SelectaleCategories { get; set; }
        public bool IsHasProducts { get; set; }
        public bool IsHasSubcategories { get; set; }
    }
}
