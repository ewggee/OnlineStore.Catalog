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
        /// Устанавливает значение <paramref name="value"/> с ключом <paramref name="key"/> без времени истечения.
        /// </summary>
        Task SetAsUnlimitedAsync<T>(string key, T value);

        /// <summary>
        /// Возвращает значение из кэша, если оно найдено. Если нет 
        /// - возвращает из переданной функции <paramref name="func"/> и, если значение есть, кэширует его.
        /// </summary>
        /// <param name="func">Функция для получения значения из другого источника.</param>
        /// <param name="lifetime">Время истечения значения в кэше.</param>
        Task<T?> GetOrSetAsync<T>(string key, Func<Task<T?>> func, TimeSpan? lifetime = null);

        /// <summary>
        /// Возвращает значение из кэша, если оно найдено. Если нет 
        /// - возвращает из переданной функции <paramref name="func"/> и, если значение есть, кэширует его на неограниченный срок.
        /// </summary>
        /// <param name="func">Функция для получения значения из другого источника.</param>
        /// <param name="lifetime">Время истечения значения в кэше.</param>
        Task<T?> GetOrSetAsUnlimitedAsync<T>(string key, Func<Task<T?>> func);

        /// <summary>
        /// Обновляет значение кэша.
        /// </summary>
        Task RefreshAsync<T>(string key, T value, TimeSpan? lifetime = null);

        /// <summary>
        /// Удаляет значение из кэша.
        /// </summary>
        Task RemoveAsync(string key);

        /// <summary>
        /// Удаляет значения из кэша.
        /// </summary>
        Task RemoveRangeAsync(string[] keys);
    }
}
