namespace OnlineStore.Catalog.Application.Abstractions
{
    public interface ICacheService
    {
        /// <summary>
        /// Возвращает значение из кэша по ключу <paramref name="key"/>.
        /// </summary>
        Task<T?> GetAsync<T>(string key);

        /// <summary>
        /// Устанавливает значение <paramref name="value"/> с ключом <paramref name="key"/>.
        /// </summary>
        /// <param name="lifetime">Время истечения значения в кэше.</param>
        Task SetAsync<T>(string key, T value, TimeSpan? lifetime = null);

        /// <summary>
        /// Пытается получить значение из кэша и возвращает его, если найдено. Если значения в кэше нет 
        /// - получает из переданной функции <paramref name="func"/> и кэширует значение, если оно получено.
        /// </summary>
        /// <param name="func">Функция для получения значения из внешнего источника.</param>
        /// <param name="lifetime">Время истечения значения в кэше.</param>
        Task<T?> GetOrSetAsync<T>(string key, Func<Task<T?>> func, TimeSpan? lifetime = null);

        /// <summary>
        /// Обновляет значение кэша.
        /// </summary>
        Task RefreshAsync<T>(string key, T value, TimeSpan? lifetime = null);

        /// <summary>
        /// Удаляет значение из кэша.
        /// </summary>
        Task RemoveAsync<T>(string key);
    }
}
