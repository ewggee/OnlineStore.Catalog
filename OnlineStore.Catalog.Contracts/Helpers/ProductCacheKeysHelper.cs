namespace OnlineStore.Catalog.Contracts.Helpers
{
    public static class ProductCacheKeysHelper
    {
        private const string Products = "Products";

        /// <summary>
        /// Возвращает каталог для товара с ID <paramref name="productId"/>.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static string ProductDir(int productId) => $"{Products}:{productId}";

        /// <summary>
        /// Возвращает строку в формате: <b>$"Products:{<paramref name="productId"/>}"</b>.
        /// </summary>
        public static string ProductById(int productId) => $"{Products}:{productId}";

        /// <summary>
        /// Возвращает строку в формате: <b>$"Categories:{<paramref name="categoryId"/>}:Products"</b>.
        /// </summary>
        public static string ProductsInCategory(int categoryId) => $"{CategoryCacheKeysHelper.CategoryDir(categoryId):Products}";
    }
}
