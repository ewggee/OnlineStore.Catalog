namespace OnlineStore.Catalog.Contracts.Dtos
{
    public class CategoryWithSubcategoriesDto
    {
        public required CategoryDto Category { get; set; }
        public required IReadOnlyList<CategoryDto> Subcategories { get; set; }
    }
}
