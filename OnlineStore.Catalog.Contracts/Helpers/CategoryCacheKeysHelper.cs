namespace OnlineStore.Catalog.Contracts.Helpers
{
    public static class CategoryCacheKeysHelper
    {
        private const string Categories = "Categories";

        /// <summary>
        /// Возвращает каталог для категории с ID <paramref name="categoryId"/>.
        /// </summary>
        public static string CategoryDir(int categoryId) => $"{Categories}:{categoryId}";

        /// <summary>
        /// Возвращает строку в формате: <b>"Categories:MainCategories"</b>.
        /// </summary>
        public const string MainCategories = $"{Categories}:MainCategories";

        /// <summary>
        /// Возвращает строку в формате: <b>$"Categories:{<paramref name="categoryId"/>}:Category}"</b>.
        /// </summary>
        public static string Category(int categoryId) => $"{CategoryDir(categoryId)}:Category";

        /// <summary>
        /// Возвращает строку в формате: <b>$"Categories:{<paramref name="categoryId"/>}:CategoryWithSubcategories}"</b>.
        /// </summary>
        public static string CategoryWithSubcategories(int categoryId) => $"{CategoryDir(categoryId)}:CategoryWithSubcategories";

        /// <summary>
        /// Возвращает строку в формате: <b>$"Categories:{<paramref name="categoryId"/>}:CategoryWithSubcategories}"</b>.
        /// </summary>
        public static string NavigationCategories(int? categoryId = null)
        {
            return categoryId.HasValue
                ? $"{Categories}:NavigationCategories"
                : $"{Categories}:NavigationCategories:{categoryId}";
        }

        /// <summary>
        /// Возвращает строку в формате: <b>"Categories:IndependentCategories"</b>.
        /// </summary>
        public const string IndependentCategories = $"{Categories}:IndependentCategories";
    }
}
