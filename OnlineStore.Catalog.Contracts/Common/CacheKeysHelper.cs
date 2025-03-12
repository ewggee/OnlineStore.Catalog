namespace OnlineStore.Catalog.Contracts.Common
{
    public static class CacheKeysHelper
    {
        /// <summary>
        /// Возвращает строку в формате: <b>$"Product:{<paramref name="key"/>}"</b>.
        /// </summary>
        public static string ProductById(string key) => $"Product:{key}";

        /// <summary>
        /// Возвращает строку в формате: <b>$"ProductsInCategory:{<paramref name="key"/>}"</b>.
        /// </summary>
        public static string ProductsInCategory(string key) => $"ProductsInCategory:{key}";
    }
}
